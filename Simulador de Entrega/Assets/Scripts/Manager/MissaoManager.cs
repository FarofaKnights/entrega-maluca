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

    public MissaoData GetMissaoData()
    {
        string[] dispoNames = new string[missoesDisponiveis.Count];
        string[] concluNames = new string[missoesConcluidas.Count];
        float[] valores = new float [missoesConcluidas.Count];
        int i = 0;
        int l = 0;
        foreach (Missao m in missoesDisponiveis)
        {
            dispoNames [i] = m.info.nome;
            i++;
        }
        foreach (Missao m in missoesConcluidas)
        {
            concluNames[l] = m.info.nome;
            valores[l] = m.melhorStatus.dinheiro;
            l++;
        }
        MissaoData md = new MissaoData(concluNames, dispoNames, valores);
        return md;
    }

    public void SetMissaoData(MissaoData md)
    {
        foreach (string m in md.missoesConcluidas)
        {
            foreach (Missao miss in missoesDisponiveis)
            {
                if(miss.info.nome == m)
                {
                    if(miss.info.missoesDesbloqueadas != null)
                    {
                        MissoesDesbloqueadas(miss.info.missoesDesbloqueadas, md);
                    }
                    RemoverMissao(miss);
                }
            }
        }
        int j = 0;
        foreach (int money in md.concluValores)
        {
            missoesConcluidas[j].melhorStatus.dinheiro = money;
            j++;
        }
    }

    public void MissoesDesbloqueadas(MissaoObject[] missoes, MissaoData md)
    {
        foreach(MissaoObject missaoObj in missoes)
        {
            Debug.Log("Z");
            Missao missao = new Missao(missaoObj);
            AdicionarMissao(missao);
            SetMissaoData(md);
        }
    }
    #region Missão runtime
    public void IniciarMissao(Missao missao) {
        if (missaoAtual != null) {
            missaoAtual.Parar();
        }

        Player.instance.ZerarCargas();
        SetEstado(Estado.Entrega);
        missaoAtual = missao;
        UIController.diretriz.SetCurrentMissao(missaoAtual);
        missaoAtual.Iniciar();

        if (OficinaController.instance != null)
            OficinaController.instance.DesativarOficina();
    }

    public void PararMissao() {
        if (missaoAtual == null) return;
        missaoAtual.Parar();

        Player.instance.ZerarCargas();
        UIController.diretriz.Close(missaoAtual);

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
        UIController.diretriz.Close(missao);
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
        PararMissao();

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

        if (missao.IsConcluida() && !missoesConcluidas.Contains(missao))
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
