using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Missao {
    public SubMissao[] submissoes; // Lista de submissoes
    int indiceSubAtual = 0; // Indice da submissao ativa
    public DestinoComecar destinoComecar; // Destino para comecar a missao

    // Construtores
    public Missao(DestinoComecar destinoComecar, SubMissao[] submissoes) {
        this.destinoComecar = destinoComecar;
        destinoComecar.missao = this;

        this.submissoes = submissoes;
        foreach (SubMissao sub in submissoes) sub.missao = this;
    }

    public Missao(Endereco enderecoComecar, SubMissao[] submissoes) {
        this.destinoComecar = new DestinoComecar(enderecoComecar, this);
        this.submissoes = submissoes;
        foreach (SubMissao sub in submissoes) sub.missao = this;
    }

    // Metodos
    public void Iniciar() {
        Player.instance.ComecarMissao(this);
        indiceSubAtual = 0;
        submissoes[indiceSubAtual].Iniciar();
    }

    public void Interromper() {
        if (Player.instance.missaoAtual == this) {
            Player.instance.missaoAtual = null;
        }

        submissoes[indiceSubAtual].Interromper();
    }

    public void ProximaSubMissao() {
        Debug.Log("Proxima SubMissao!");
        indiceSubAtual++;

        if (indiceSubAtual >= submissoes.Length) {
            Concluir();
        } else {
            submissoes[indiceSubAtual].Iniciar();
        }
    }

    void Concluir() {
        Debug.Log("Concluiu!");
        Player.instance.dinheiro += 100;
        Player.instance.FinalizarMissao();

        UIController.instance.MissaoConcluida();

        // Gera nova missao no final
        Missao novaMissao = GerarMissaoAleatoria();
        Player.instance.AdicionarMissao(novaMissao);
    }

    #region Aleatoria
    // Gera uma missão A->B aleatoria
    public static Missao GerarMissaoAleatoria() {
        int a, b;
        a = Random.Range(1,6); // de 1 a 5

        // Gera numero aleatorio diferente de a
        do {
            b = Random.Range(1,6);
        } while (a == b);

        // Pega objetos do tipo Endereco
        Endereco remetente = Endereco.ListaEnderecos["Predio " + a];
        Endereco destinatario = Endereco.ListaEnderecos["Predio " + b];

        // Gera quantidade aleatoria de cargas
        List<Carga> cargas = new List<Carga>();
        int quant = Random.Range(1, 4);
        for (int i = 0; i < quant; i++) {
            Carga carga = new Carga(1, 1, remetente);
            cargas.Add(carga);
        }

        // Define padrão de destino de A a B
        Destino final = new Destino(destinatario);
        final.permiteReceber = true;

        DestinoComecar inicio = new DestinoComecar(remetente, cargas);

        Destino[] destinos = new Destino[1] {final};
        SubMissao submissao = new SubMissao(null, destinos, true);
        return new Missao(inicio, new SubMissao[1] {submissao});
    }
    
    // Gera uma missão de 3 pontos aleatoria
    public static Missao GerarMissaoMultiplosPontos() {
        int[] nums = XNumerosAleatorioSemRepetir(4, 1, 5);
        Destino[] destinos = new Destino[3];
        SubMissao submissao = new SubMissao(null, destinos, false);
        List<Carga> cargas = new List<Carga>();

        for (int i = 0; i < nums.Length - 1; i++) {
            Endereco endereco = Endereco.ListaEnderecos["Predio " + nums[i+1]];
            destinos[i] = new Destino(endereco, submissao);
            destinos[i].permiteReceber = true;

            Carga carga = new Carga(1, 1, endereco);
            cargas.Add(carga);
        }

        DestinoComecar inicio = new DestinoComecar(Endereco.ListaEnderecos["Predio " + nums[0]], cargas);

        return new Missao(inicio, new SubMissao[1] {submissao});
    }
    
    // Gera um array de numeros aleatorios sem repetir
    public static int[] XNumerosAleatorioSemRepetir(int quant, int min, int max) {
        int[] arr = new int[quant];
        bool repete;

        for (int i=0; i < arr.Length; i++) {
            int rand;
            do {
                repete = false;
                rand = Random.Range(min, max + 1);

                for (int j = 0; j < i; j++) {
                    if (rand == arr[j]){
                        repete = true;
                        break;
                    }
                }
            } while (repete);

            arr[i] = rand;
            Debug.Log(rand);
        }

        return arr;
    }
    #endregion

    #region Conversoes
    // Converte objeto MissaoObject em uma Missao
    public static Missao GerarMissao(MissaoObject missaoObject) {
        // Gera o destino inicial da missão
        Destino comecar = GerarDestino(missaoObject.destinoComecar);
        DestinoComecar destinoComecar = new DestinoComecar(comecar.endereco, comecar.cargas);

        SubMissao[] submissoes = new SubMissao[missaoObject.submissoes.Length];

        for (int i = 0; i < missaoObject.submissoes.Length; i++) {
            SubMissaoObject submissaoObject = missaoObject.submissoes[i];
            Destino[] destinos = new Destino[submissaoObject.destinos.Length];

            for (int j = 0; j < submissaoObject.destinos.Length; j++) {
                DestinoObject destinoObject = submissaoObject.destinos[j];
                Destino destino = GerarDestino(destinoObject);
                destinos[j] = destino;
            }

            SubMissao submissao = new SubMissao(null, destinos, submissaoObject.sequencial);
            submissoes[i] = submissao;
        }

        return new Missao(destinoComecar, submissoes);
    }

    // Converte objeto DestinoObject em um Destino
    public static Destino GerarDestino(DestinoObject destinoObject) {
        Endereco endereco = Endereco.GetEndereco(destinoObject.endereco);
        List<Carga> cargas = null;

        if (destinoObject.cargas != null) {
            cargas  = new List<Carga>();
            foreach (CargaObject cargaObject in destinoObject.cargas) {
                Carga carga = GerarCarga(cargaObject);
                cargas.Add(carga);
            }
        }

        Destino destino = new Destino(endereco, cargas);
        destino.permiteReceber = destinoObject.permiteReceber;
        return destino;
    }

    // Converte objeto CargaObject em uma Carga
    public static Carga GerarCarga(CargaObject cargaObject) {
        Endereco endereco = Endereco.GetEndereco(cargaObject.destinatario);
        Carga carga = new Carga(cargaObject.peso, cargaObject.fragilidade, endereco, cargaObject.tipo);
        return carga;
    }
    #endregion
}
