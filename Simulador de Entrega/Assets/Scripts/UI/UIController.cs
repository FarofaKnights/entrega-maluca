using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIController : MonoBehaviour {
    public static UIController instance;

    public Button botaoAcao, botaoConfirm, botaoReiniciarTetris;
    public Text textoDiretriz;
    public GameObject textoMissaoConcluida;

    public GameObject missaoPanel, diretrizPanel;
    public GameObject refMissaoPanel, refEncaixePanel, refOficinaPanel, refPausaPanel, refMinimapaPanel;

    public GameObject tutorialEncaixeMovimento, tutorialEncaixeRotacao;
    bool estaNoEncaixe = false; // Solucao temporaria

    public AudioMixer audioMixer;
    public Slider efeitoSlider, carroSlider, geralSlider;

    public Text dinheiro;

    public Objetivo objetivo;

    List<Diretriz> diretrizes = new List<Diretriz>();

    void Start() {
        instance = this;

        textoMissaoConcluida.SetActive(false);

        diretrizPanel.SetActive(false);

        botaoAcao.gameObject.SetActive(false);
        botaoAcao.onClick.AddListener(delegate { HandleBotaoAcao();});

        botaoReiniciarTetris.gameObject.SetActive(false);
        botaoReiniciarTetris.onClick.AddListener(delegate { Cacamba.instance.ReiniciarTetris(); });

        missaoPanel.SetActive(false);

        MostrarTelaMissao();
        botaoConfirm.onClick.AddListener(delegate { Confirm(Cacamba.instance.cargas); });

        AtualizarDinheiro();
    }

    void FixedUpdate() {
        if (estaNoEncaixe && Cacamba.instance.caixaAtual != null) {
            Caixas caixa = Cacamba.instance.caixaAtual;
            if (caixa != null) {
                bool estaRodando = caixa.rodando;
                tutorialEncaixeMovimento.SetActive(!estaRodando);
                tutorialEncaixeRotacao.SetActive(estaRodando);
            }
        }
    }

    // Chamado quando player entra/sai em uma área de Acao
    public void PlayerNaAreaDeAcao(Objetivo objetivo, bool estado) {
        this.objetivo = objetivo;
        botaoAcao.gameObject.SetActive(estado);
    }

    // Chamado quando player entra/sai em uma área de Iniciar Missão
    public void PlayerNaAreaDeIniciarMissao(ObjetivoInicial objetivo, bool estado) {
        this.objetivo = objetivo;
        missaoPanel.SetActive(estado);

        if (estado) {
            Text titulo = missaoPanel.transform.Find("Titulo").GetComponent<Text>();
            Text descricao = missaoPanel.transform.Find("Descricao").GetComponent<Text>();

            titulo.text = objetivo.missao.titulo;
            descricao.text = objetivo.missao.descricao;
        }
    }

    // Handle do clique no botão "Fazer Ação"
    public void HandleBotaoAcao() {
        objetivo.Concluir();
        botaoAcao.gameObject.SetActive(false);
    }

    // Handle do clique no botão "Iniciar Missão"
    public void HandleBotaoIniciarMissao() {
        objetivo.Concluir();
        missaoPanel.SetActive(false);
        refMinimapaPanel.SetActive(false);
    }

    public void MissaoConcluida() {
        textoMissaoConcluida.SetActive(true);

        // Espera 2 segundos e esconde o texto
        StartCoroutine(EsconderTextoMissaoConcluida());
    }

    IEnumerator EsconderTextoMissaoConcluida() {
        yield return new WaitForSeconds(2);
        textoMissaoConcluida.SetActive(false);
    }
    public void Confirm(Caixas[] c)
    {
         Cacamba.instance.MudarCaixas(c);
    }

    public void InterromperTetris() {
        // Solução temporária para o caos do startdrag
        Cacamba.instance.FinalizarTetris();
        MostrarTelaMissao();
    }

    public void MostrarTelaEncaixe() {
        refEncaixePanel.SetActive(true);
        refMinimapaPanel.SetActive(false);
        estaNoEncaixe = true;

        tutorialEncaixeMovimento.SetActive(true);
        tutorialEncaixeRotacao.SetActive(false);
    }

    public void MostrarTelaMissao() {
        refEncaixePanel.SetActive(false);
        refMinimapaPanel.SetActive(true);
        estaNoEncaixe = false;
    }

    #region Diretriz

    public void AdicionarDiretriz(Diretriz diretriz) {
        diretrizes.Add(diretriz);
        diretrizPanel.SetActive(true);

        AtualizaTextoDiretrizes();
    }

    public void RemoverDiretriz(Diretriz diretriz) {
        diretrizes.Remove(diretriz);

        if (diretrizes.Count == 0)
            diretrizPanel.SetActive(false);
        
        AtualizaTextoDiretrizes();
    }

    void AtualizaTextoDiretrizes() {
        string text = "";
        int i = 0;

        foreach (Diretriz d in diretrizes) {
            text += d.texto;

            if (i < diretrizes.Count - 1)
                text += "\n";
            
            i++;
        }
        textoDiretriz.text = text;
    }

    #endregion

    public void EntrarOficina() {
        refOficinaPanel.SetActive(true);
        refMissaoPanel.SetActive(false);
        refEncaixePanel.SetActive(false);
        refMinimapaPanel.SetActive(false);

        refOficinaPanel.GetComponent<OficinaUI>().HandleMostrarGrid();
    }

    public void SairOficina() {
        refOficinaPanel.SetActive(false);
        refMissaoPanel.SetActive(true);
        refEncaixePanel.SetActive(true);
        refMinimapaPanel.SetActive(true);
    }

    public void AtualizarDinheiro() {
        dinheiro.text = Player.instance.GetDinheiro().ToString("C2");
    }

    public void EntrarPausa() {
        refPausaPanel.SetActive(true);
        refPausaPanel.GetComponent<MenuPauseController>().OpenMenu();
    }

    public void SairPausa() {
        refPausaPanel.SetActive(false);
    }

    public void SetVolumeEfeito(float volume) {
        audioMixer.SetFloat("Efeitos", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeCarro(float volume) {
        audioMixer.SetFloat("Carro", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeGeral(float volume) {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);

        Debug.Log("Volume geral: " + volume);
        Debug.Log("Volume geral (log): " + Mathf.Log10(volume));
        Debug.Log("Volume geral (log) * 20: " + Mathf.Log10(volume) * 20);

        audioMixer.GetFloat("Master", out volume);
        Debug.Log("Volume geral (log) * 20 (int): " + volume);
    }

    public void UpdateSliders() {
        float volume;
        audioMixer.GetFloat("Efeitos", out volume);
        efeitoSlider.value = Mathf.Pow(10, volume / 20);

        audioMixer.GetFloat("Carro", out volume);
        carroSlider.value = Mathf.Pow(10, volume / 20);

        audioMixer.GetFloat("Master", out volume);
        geralSlider.value = Mathf.Pow(10, volume / 20);
    }
}
