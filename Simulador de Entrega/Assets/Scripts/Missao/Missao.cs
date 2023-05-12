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

    // Construtores
    public Missao(ObjetivoInicial objetivoInicial, Conjunto[] conjuntos) {
        this.objetivoInicial = objetivoInicial;
        this.conjuntos = conjuntos;

        objetivoInicial.missao = this;
        foreach (Conjunto conjunto in conjuntos) conjunto.missao = this;
    }

    public Missao(Endereco enderecoComecar, Conjunto[] conjuntos) {
        this.objetivoInicial = new ObjetivoInicial(enderecoComecar, this);
        this.conjuntos = conjuntos;

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
        Player.instance.AdicionarMissao(novaMissao);
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
        return new Missao(inicio, new Conjunto[1] {conjunto});
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

        return new Missao(inicio, new Conjunto[1] {conjunto});
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
        // Gera o objetivo inicial da missão
        Objetivo comecar = GerarObjetivo(missaoObject.objetivoInicial);
        ObjetivoInicial objetivoInicial = new ObjetivoInicial(comecar.endereco, comecar.cargas);

        Conjunto[] conjuntos = new Conjunto[missaoObject.conjuntos.Length];

        for (int i = 0; i < missaoObject.conjuntos.Length; i++) {
            ConjuntoObject conjuntoObject = missaoObject.conjuntos[i];
            Objetivo[] objetivos = new Objetivo[conjuntoObject.objetivos.Length];

            for (int j = 0; j < conjuntoObject.objetivos.Length; j++) {
                ObjetivoObject objetivoObject = conjuntoObject.objetivos[j];
                Objetivo objetivo = GerarObjetivo(objetivoObject);
                objetivos[j] = objetivo;
            }

            Conjunto conjunto = new Conjunto(null, objetivos, conjuntoObject.sequencial);
            conjuntos[i] = conjunto;
        }

        return new Missao(objetivoInicial, conjuntos);
    }

    // Converte objeto ObjetivoObject em um Objetivo
    public static Objetivo GerarObjetivo(ObjetivoObject objetivoObject) {
        Endereco endereco = Endereco.GetEndereco(objetivoObject.endereco);
        List<Carga> cargas = null;

        if (objetivoObject.cargas != null) {
            cargas  = new List<Carga>();
            foreach (CargaObject cargaObject in objetivoObject.cargas) {
                Carga carga = GerarCarga(cargaObject);
                cargas.Add(carga);
            }
        }

        Objetivo objetivo = new Objetivo(endereco, cargas);
        objetivo.permiteReceber = objetivoObject.permiteReceber;
        return objetivo;
    }

    // Converte objeto CargaObject em uma Carga
    public static Carga GerarCarga(CargaObject cargaObject) {
        Endereco endereco = Endereco.GetEndereco(cargaObject.destinatario);
        Carga carga = new Carga(cargaObject.peso, cargaObject.fragilidade, endereco, cargaObject.tipo);
        return carga;
    }
    #endregion
}
