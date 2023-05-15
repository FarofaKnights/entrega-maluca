using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Missao: Iniciavel {
    public Conjunto[] conjuntos; // Lista de conjuntos

    [SerializeField]
    int indiceConjunto = 0; // Indice do conjunto ativo
    
    public ObjetivoInicial objetivoInicial; // Objetivo para comecar a missao

    List<Carga> cargasEntregues = new List<Carga>(); // Lista de cargas entregues

    public string titulo, descricao;
    public Diretriz diretriz = null;

    // Construtores
    public Missao(ObjetivoInicial objetivoInicial, Conjunto[] conjuntos, string titulo, string descricao) {
        this.objetivoInicial = objetivoInicial;
        this.conjuntos = conjuntos;
        this.titulo = titulo;
        this.descricao = descricao;

        objetivoInicial.missao = this;
        foreach (Conjunto conjunto in conjuntos) conjunto.missao = this;
    }

    public Missao(Endereco enderecoComecar, Conjunto[] conjuntos, string titulo, string descricao) {
        this.objetivoInicial = new ObjetivoInicial(enderecoComecar, this);
        this.conjuntos = conjuntos;
        this.titulo = titulo;
        this.descricao = descricao;

        foreach (Conjunto conjunto in conjuntos) conjunto.missao = this;
    }

    // Metodos Iniciavel
    public void Iniciar() {
        Player.instance.ComecarMissao(this);

        indiceConjunto = 0;
        conjuntos[indiceConjunto].Iniciar();
    }

    public void Interromper() {
        if (Player.instance.missaoAtual == this) {
            Player.instance.missaoAtual = null;
        }

        conjuntos[indiceConjunto].Interromper();
        indiceConjunto = 0;
    }

    public void Finalizar() {
        float dinheiro = 0;

        foreach (Carga carga in cargasEntregues) {
            dinheiro += carga.GetValor();
        }

        Player.instance.AdicionarDinheiro(dinheiro);
        Player.instance.FinalizarMissao();

        UIController.instance.MissaoConcluida();
        indiceConjunto = 0;

        // Gera nova missao no final
        Missao novaMissao = GerarMissaoAleatoria();
        GameManager.instance.AdicionarMissao(novaMissao);
    }

    // Metodos Missao
    public void ProximoConjunto() {
        indiceConjunto++;

        if (indiceConjunto >= conjuntos.Length) {
            Finalizar();
        } else {
            conjuntos[indiceConjunto].Iniciar();
        }
    }

    public void CargaEntregue(Carga carga) {
        cargasEntregues.Add(carga);
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

        // Define padrão de objetivo de A a B
        Objetivo final = new Objetivo(destinatario);
        final.permiteReceber = true;

        ObjetivoInicial inicio = new ObjetivoInicial(remetente, cargas);

        Objetivo[] objetivos = new Objetivo[1] {final};
        Conjunto conjunto = new Conjunto(null, objetivos, true);
        return new Missao(inicio, new Conjunto[1] {conjunto}, "Missão Aleatória", "Entregue as cargas no prédio " + b + ".");
    }
    
    // Gera uma missão de 3 pontos aleatoria
    public static Missao GerarMissaoMultiplosPontos() {
        int[] nums = XNumerosAleatorioSemRepetir(4, 1, 5);
        Objetivo[] objetivos = new Objetivo[3];
        Conjunto conjunto = new Conjunto(null, objetivos, false);
        List<Carga> cargas = new List<Carga>();

        for (int i = 0; i < nums.Length - 1; i++) {
            Endereco endereco = Endereco.ListaEnderecos["Predio " + nums[i+1]];
            objetivos[i] = new Objetivo(endereco, conjunto);
            objetivos[i].permiteReceber = true;

            Carga carga = new Carga(1, 1, endereco);
            cargas.Add(carga);
        }

        ObjetivoInicial inicio = new ObjetivoInicial(Endereco.ListaEnderecos["Predio " + nums[0]], cargas);

        return new Missao(inicio, new Conjunto[1] {conjunto}, "Missão Aleatória", "Entregue as cargas nos prédios " + nums[1] + ", " + nums[2] + " e " + nums[3] + ".");
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
}
