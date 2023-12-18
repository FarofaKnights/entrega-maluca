using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour {
    public GameObject telaMain, telaFalha, telaSucesso;

    public TelaVitoriaUI vitoria;

    // Missão
    public Button botaoAcao, botaoReiniciarTetris;
    public GameObject missaoPanel;
    public Objetivo objetivo;
    Missao missao;
    Timer currentTimer = null;

    public TextMeshProUGUI tituloMissao, motivoDaFalha;
    public Text descricaoMissao;
    public Image imagemMissao;

    public TextMeshProUGUI timer;

    void Awake() {
        vitoria = transform.GetComponentInChildren<TelaVitoriaUI>(true);
    }

    void Start() {
        botaoAcao.gameObject.SetActive(false);
        botaoAcao.onClick.AddListener(delegate { HandleBotaoAcao(); });

        botaoReiniciarTetris.gameObject.SetActive(false); 
        botaoReiniciarTetris.onClick.AddListener(delegate { HandleBotaoRecuperar(); }); 

        missaoPanel.SetActive(false);

        UIController.dinheiro.AtualizarDinheiro();

        if (UIController.encaixe.gameObject.activeSelf) {
            Debug.Log("Encaxie está ligado logo eu vou me desligar!!!");
            gameObject.SetActive(false);
        }
    }

    public void ChamaVitoria(Missao missao, StatusMissao status) {
        vitoria.gameObject.SetActive(true);
        vitoria.MissaoConcluida(missao, status);
    }

    #region Missão

    // Chamado quando player entra/sai em uma área de Acao
    public void MostrarBotaoAcao(Objetivo objetivo, bool mostrar) {
        this.objetivo = mostrar ? objetivo : null;
        botaoAcao.gameObject.SetActive(mostrar);
    }

    // Chamado quando player entra/sai em uma área de Iniciar Missão
    public void MostrarMissaoInfo(ObjetivoInicial objetivoInicial, bool mostrar) {
        missaoPanel.SetActive(mostrar);

        if (mostrar) {
            this.objetivo = objetivoInicial;

            tituloMissao.text = objetivoInicial.missao.info.nome;
            descricaoMissao.text = objetivoInicial.missao.info.descricao;
            imagemMissao.sprite = objetivoInicial.missao.info.GetEnderecoInicial().GetImagem();
        } else {
            this.objetivo = null;
        }
    }

    // Handle do clique no botão "Fazer Ação"
    [SerializeField]
    void HandleBotaoAcao() {
        if (objetivo != null) objetivo.Concluir();
        MostrarBotaoAcao(null, false);
    }

    // Handle do clique no botão "Iniciar Missão"
    public void HandleBotaoIniciarMissao() {
        if (objetivo != null) objetivo.Concluir();
        MostrarMissaoInfo(null, false);
    }

    public void FalhaMissao(Missao missao, string motivo) {
        this.missao = missao;
        telaFalha.SetActive(true);
        Time.timeScale = 0;

        motivoDaFalha.text = motivo;
    }

    public void SairFalhaMissao() {
        telaFalha.SetActive(false);
        Time.timeScale = 1;
    }

    public void ResetarMissao() {
        telaFalha.SetActive(false);
        Time.timeScale = 1;

        MissaoManager.instance.ReiniciarMissao(missao);
    }

    #endregion

    #region Tetris
    
    public void MostrarBotaoRecuperar(bool mostrar) {
        botaoReiniciarTetris.gameObject.SetActive(mostrar);
    }

    void HandleBotaoRecuperar() {
        Player.instance.RecuperarCargasProximas();
        MostrarBotaoRecuperar(false);
    }

    #endregion

    

    public void MostrarTimer(Timer timer) {
        currentTimer = timer;
        this.timer.transform.parent.gameObject.SetActive(true);
    }

    public void EsconderTimer(Timer timer) {
        if (currentTimer != timer) return;
        this.timer.transform.parent.gameObject.SetActive(false);
    }

    public void AtualizarTimer(float tempo) {
        int minutos = (int) tempo / 60;
        int segundos = (int) tempo % 60;

        timer.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }
}
