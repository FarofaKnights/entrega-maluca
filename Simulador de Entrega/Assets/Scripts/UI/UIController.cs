using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class UIController : MonoBehaviour {
    public static UIController instance;
    public static HUDController HUD;

    public Button botaoAcao, botaoConfirm;
    
    

    
    public GameObject refMissaoPanel, refEncaixePanel, refOficinaPanel, refPausaPanel, refMinimapaPanel;

    public GameObject tutorialEncaixeMovimento, tutorialEncaixeRotacao;
    bool estaNoEncaixe = false; // Solucao temporaria

    public AudioMixer audioMixer;
    public Slider efeitoSlider, carroSlider, geralSlider;

    
    void Awake() {
        instance = this;
        HUD = transform.GetComponentInChildren<HUDController>()[0];
    }
    

    void Start() {
        instance = this;

        textoMissaoConcluida.SetActive(false);

        diretrizPanel.SetActive(false);

        botaoAcao.gameObject.SetActive(false);
        botaoAcao.onClick.AddListener(delegate { HandleBotaoAcao();});

        missaoPanel.SetActive(false);

        MostrarTelaMissao();
        botaoConfirm.onClick.AddListener(delegate { Confirm(); });

        AtualizarDinheiro();
    }

    void FixedUpdate() {
        if (estaNoEncaixe && Cacamba.instance.objSelecionado != null) {
            GameObject obj = Cacamba.instance.objSelecionado;
            Caixas caixa = obj.GetComponent<Caixas>();
            if (caixa != null) {
                bool estaRodando = caixa.rodando;
                tutorialEncaixeMovimento.SetActive(!estaRodando);
                tutorialEncaixeRotacao.SetActive(estaRodando);
            }
        }
    }

    // Handle do clique no botão "Fazer Ação"
    public void HandleBotaoAcao() {
        objetivo.Concluir();
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
    public void Confirm()
    {
        if (Cacamba.instance.completed)
        {
            Cacamba.instance.FinalizarTetris();
            objetivo.Finalizar();
            MostrarTelaMissao();
        }
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
