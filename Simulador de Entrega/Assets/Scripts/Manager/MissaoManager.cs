using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissaoManager : MonoBehaviour {
    public static MissaoManager instance;

    public enum Estado {SemMissao, Entrega};
    public Estado estado = Estado.SemMissao;

    public MissaoObject[] missoesIniciais;
    List<Missao> missoesDisponiveis = new List<Missao>();
    List<Missao> missoesConcluidas = new List<Missao>();

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


    #region Missão runtime
    public void IniciarMissao(Missao missao) {
        if (missaoAtual != null) {
            missaoAtual.Parar();
        }

        Player.instance.ZerarCargas();
        SetEstado(Estado.Entrega);
        missaoAtual = missao;
        missaoAtual.Iniciar();

        if (OficinaController.instance != null)
            OficinaController.instance.DesativarOficina();
    }

    public void PararMissao() {
        if (missaoAtual == null) return;
        missaoAtual.Parar();

        Player.instance.ZerarCargas();

        missaoAtual = null;
        SetEstado(Estado.SemMissao);

        if(Player.instance.GetState().GetType() == typeof(EncaixeState))
            Player.instance.SetDirigindo();

        if (OficinaController.instance != null)
            OficinaController.instance.AtivarOficina();
    }

    public void HandleMissaoConcluida(Missao missao) {
        if (!missao.IsConcluida()) return;

        RemoverMissao(missao);
        missaoAtual = null;

        Player.instance.ZerarCargas();
        SetEstado(Estado.SemMissao);

        if (OficinaController.instance != null)
            OficinaController.instance.AtivarOficina();

        if (missao.info.missoesDesbloqueadas != null && missao.info.missoesDesbloqueadas.Length > 0) {
            foreach (MissaoObject missaoObject in missao.info.missoesDesbloqueadas) {
                AdicionarMissao(new Missao(missaoObject));
            }
        }
    }

    public void ReiniciarMissao(Missao missao) {
        Objetivo obj = missao.GetObjetivoInicial();
        obj.endereco.TeleportToHere();

        obj.Concluir();
    }

    public void SetEstado(Estado estado) {
        Estado estadoAntigo = this.estado;

        this.estado = estado;

        bool mostrar = estado == Estado.SemMissao;
        bool mostrarAntigo = estadoAntigo == Estado.SemMissao;

        if (mostrar != mostrarAntigo) {
            foreach (Missao missao in missoesDisponiveis) {
                missao.ShowObjetivoInicial(mostrar);
            }
        }
    }
    
    #endregion

    #region Carregar Missoes
    void CarregarMissaoInicial() {
        foreach (MissaoObject missaoObject in missoesIniciais) {
            Missao missao = new Missao(missaoObject);
            AdicionarMissao(missao);
        }
    }

    public void AdicionarMissao(Missao missao) {
        missoesDisponiveis.Add(missao);

        if (estado == Estado.SemMissao)
            missao.ShowObjetivoInicial(true);
    }

    public void RemoverMissao(Missao missao) {
        missoesDisponiveis.Remove(missao);

        if (missao.IsConcluida())
            missoesConcluidas.Add(missao);

        if (estado == Estado.SemMissao)
            missao.ShowObjetivoInicial(false);
    }
    
    
    public Missao[] GetMissoesDisponiveis() {
        return missoesDisponiveis.ToArray();
    }

    public Missao[] GetMissoesConcluidas() {
        return missoesConcluidas.ToArray();
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
