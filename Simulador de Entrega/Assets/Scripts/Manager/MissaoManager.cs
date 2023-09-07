using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissaoManager : MonoBehaviour {
    public static MissaoManager instance;

    public enum Estado {SemMissao, Entrega};
    public Estado estado = Estado.SemMissao;

    public MissaoObject[] missoesIniciais;

    public List<Missao> missoesDisponiveis = new List<Missao>();
    public List<Missao> missoesConcluidas = new List<Missao>();

    public List<Carga> cargaAtual = new List<Carga>(); // Sistema temporario
    public List<Objetivo> objetivosAtivos = new List<Objetivo>();

    [System.NonSerialized]
    public Missao missaoAtual = null;

    void Awake () {
        instance = this;
    }

    void Start() {
        foreach (KeyValuePair<string, Endereco> entry in Endereco.ListaEnderecos) {
            if (entry.Value.GetType() != typeof(EnderecoFalso))
                entry.Value.gameObject.SetActive(false);
        }

        CarregarMissaoInicial();
    }

    public void SetEstado(Estado estado) {
        Estado estadoAntigo = this.estado;

        this.estado = estado;

        bool mostrar = estado == Estado.SemMissao;
        bool mostrarAntigo = estadoAntigo == Estado.SemMissao;

        if (mostrar != mostrarAntigo) {
            foreach (Missao missao in missoesDisponiveis) {
                if (mostrar) missao.objetivoInicial.Iniciar();
                else missao.objetivoInicial.Interromper();
            }
        }
    }
    
    #region Carregar Missoes
    void CarregarMissaoInicial() {
        foreach (MissaoObject missaoObject in missoesIniciais) {
            Missao missao = missaoObject.Convert();
            AdicionarMissao(missao);
        }
    }

    public void AdicionarMissao(Missao missao) {
        missoesDisponiveis.Add(missao);

        if (estado == Estado.SemMissao)
            missao.objetivoInicial.Iniciar();
    }

    public void RemoverMissao(Missao missao) {
        missoesDisponiveis.Remove(missao);

        if (missao.FoiFinalizada())
            missoesConcluidas.Add(missao);

        if (estado == Estado.SemMissao)
            missao.objetivoInicial.Interromper();
    }
    #endregion

    #region Missao Runtime
    public void ComecarMissao(Missao missao) {
        if (missaoAtual != null)
            missaoAtual.Interromper();
        
        ZerarCargas();
        
        missaoAtual = missao;
        SetEstado(Estado.Entrega);

        missaoAtual.Iniciar();

        if (OficinaController.instance != null)
            OficinaController.instance.DesativarOficina();
    }

    public void InterromperMissao() {
        if (missaoAtual != null)
            missaoAtual.Interromper();

        ZerarCargas();

        missaoAtual = null;
        SetEstado(Estado.SemMissao);

        if (OficinaController.instance != null)
            OficinaController.instance.AtivarOficina();
    }

    // Chamada pela propria missao ao ser finalizada
    public void FinalizarMissao() {
        RemoverMissao(missaoAtual);

        ZerarCargas();

        missaoAtual = null;
        SetEstado(Estado.SemMissao);

        if (OficinaController.instance != null)
            OficinaController.instance.AtivarOficina();
    }

    #endregion

    #region Sistema De Carga

    // Metodo chamado por um Objetivo que esta enviando uma carga
    public void AdicionarCarga(List<Carga> cargas) {
        cargaAtual.AddRange(cargas);

        Cacamba.instance.IniciarTetris();
    }

    // Metodo chamado por um Objetivo que esta recebendo uma carga
    public List<Carga> RemoverCarga(Endereco endereco) {
        List<Carga> cargasRemovidas = new List<Carga>();

        // Poderia ser substituido por um Where ?
        for (int i = cargaAtual.Count-1; i >= 0; i--) {
            Carga carga = cargaAtual[i];
            if (carga.destinatario == endereco) {
                carga.cx.Remover();
                cargasRemovidas.Add(carga);
                cargaAtual.Remove(carga);
            }
        }

        return cargasRemovidas;
    }

    // Zera todas as cargas
    public void ZerarCargas() {
        foreach (Carga carga in cargaAtual) {
            carga.cx.Remover();
        }

        cargaAtual = new List<Carga>();
    }

    #endregion

    #region Objetivos

    public void AddObjetivoAtivo(Objetivo objetivo) {
        if (objetivosAtivos.Contains(objetivo)) return;
        if (objetivo.GetType() == typeof(ObjetivoInicial)) return;

        objetivosAtivos.Add(objetivo);
    }

    public void RemoveObjetivoAtivo(Objetivo objetivo) {
        if (!objetivosAtivos.Contains(objetivo)) return;

        objetivosAtivos.Remove(objetivo);
    }

    #endregion
}
