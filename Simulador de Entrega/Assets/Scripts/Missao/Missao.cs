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

    public MissaoObject[] missoesDesbloqueadas; // Lista de missoes desbloqueadas ao finalizar

    public string titulo, descricao;
    public Diretriz diretriz = null;

    public bool gerarAleatoriaNoFinal = false;

    bool iniciada = false;
    bool finalizada = false;

    // Construtores
    public Missao(ObjetivoInicial objetivoInicial, Conjunto[] conjuntos, string titulo, string descricao, MissaoObject[] missoesDesbloqueadas = null, bool gerarAleatoriaNoFinal = false) {
        this.objetivoInicial = objetivoInicial;
        this.conjuntos = conjuntos;
        this.titulo = titulo;
        this.descricao = descricao;
        this.missoesDesbloqueadas = missoesDesbloqueadas;
        this.gerarAleatoriaNoFinal = gerarAleatoriaNoFinal;

        objetivoInicial.missao = this;
        foreach (Conjunto conjunto in conjuntos) conjunto.missao = this;
    }

    public Missao(Endereco enderecoComecar, Conjunto[] conjuntos, string titulo, string descricao, MissaoObject[] missoesDesbloqueadas = null, bool gerarAleatoriaNoFinal = false) {
        this.objetivoInicial = new ObjetivoInicial(enderecoComecar, this);
        this.conjuntos = conjuntos;
        this.titulo = titulo;
        this.descricao = descricao;
        this.missoesDesbloqueadas = missoesDesbloqueadas;
        this.gerarAleatoriaNoFinal = gerarAleatoriaNoFinal;

        foreach (Conjunto conjunto in conjuntos) conjunto.missao = this;
    }

    // Metodos Iniciavel
    public void Iniciar() {
        iniciada = true;

        indiceConjunto = 0;
        conjuntos[indiceConjunto].Iniciar();
    }

    public void Interromper() {
        if (!iniciada) return;

        if (MissaoManager.instance.missaoAtual == this) {
            MissaoManager.instance.missaoAtual = null;
        }
        
        iniciada = false;

        conjuntos[indiceConjunto].Interromper();
        indiceConjunto = 0;

        if(Cacamba.instance.currentState == Cacamba.State.Tetris)
            UIController.encaixe.InterromperTetris();

        MissaoManager.instance.HandleMissaoInterrompida();
    }

    public void Finalizar() {
        float dinheiro = 0;

        foreach (Carga carga in cargasEntregues) {
            dinheiro += carga.GetValor();
        }

        Player.instance.AdicionarDinheiro(dinheiro);

        finalizada = true;
        MissaoManager.instance.FinalizarMissao();

        UIController.HUD.MissaoConcluida();
        indiceConjunto = 0;

        if (missoesDesbloqueadas != null && missoesDesbloqueadas.Length > 0) {
            foreach (MissaoObject missaoObject in missoesDesbloqueadas) {
                Missao missao = missaoObject.Convert();
                MissaoManager.instance.AdicionarMissao(missao);
            }
        } else if (gerarAleatoriaNoFinal) {
            Missao novaMissao = GerarMissaoAleatoria();
            MissaoManager.instance.AdicionarMissao(novaMissao);
        }
    }

    public void Resetar() {
        Endereco e = objetivoInicial.endereco;
        e.TeleportToHere();

        objetivoInicial.Concluir();
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

    public bool FoiFinalizada() {
        return finalizada;
    }

    public bool FoiIniciada() {
        return iniciada;
    }

    #region Aleatoria
    // Gera uma missão A->B aleatoria
    public static Missao GerarMissaoAleatoria() {
        // Pega objetos do tipo Endereco
        Endereco remetente = Endereco.GetRandomEndereco();
        Endereco destinatario;

        do {
            destinatario = Endereco.GetRandomEndereco();
        } while (remetente == destinatario);

        // Gera quantidade aleatoria de cargas
        List<Carga> cargas = new List<Carga>();
        int quant = Random.Range(1, 4);
        for (int i = 0; i < quant; i++) {
            Carga carga = new Carga(1, 1, destinatario);
            cargas.Add(carga);
        }

        // Define padrão de objetivo de A a B
        Objetivo final = new Objetivo(destinatario);
        final.permiteReceber = true;

        Diretriz dir = new Diretriz("Entregue as cargas no " + destinatario.nome + ".");
        ObjetivoInicial inicio = new ObjetivoInicial(remetente, cargas);

        Objetivo[] objetivos = new Objetivo[1] {final};
        Conjunto conjunto = new Conjunto(null, objetivos, true);
        conjunto.diretriz = dir;

        return new Missao(inicio, new Conjunto[1] {conjunto}, "Missão Aleatória", "Entregue as cargas no " + destinatario.nome + ".", null, true);
    }
    
    // Gera uma missão de 3 pontos aleatoria
    public static Missao GerarMissaoMultiplosPontos() {
        int[] nums = XNumerosAleatorioSemRepetir(3, 1, 3);
        Objetivo[] objetivos = new Objetivo[3];
        Conjunto conjunto = new Conjunto(null, objetivos, false);
        List<Carga> cargas = new List<Carga>();

        for (int i = 0; i < nums.Length - 1; i++) {
            Endereco endereco = Endereco.ListaEnderecos["Predio" + nums[i+1]];
            objetivos[i] = new Objetivo(endereco, conjunto);
            objetivos[i].permiteReceber = true;

            Carga carga = new Carga(1, 1, endereco);
            cargas.Add(carga);
        }

        Diretriz dir = new Diretriz("Entregue as cargas nos prédios " + nums[1] + ", " + nums[2] + " e " + nums[3] + ".");
        ObjetivoInicial inicio = new ObjetivoInicial(Endereco.ListaEnderecos["Predio" + nums[0]], cargas);
        conjunto.diretriz = dir;

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
