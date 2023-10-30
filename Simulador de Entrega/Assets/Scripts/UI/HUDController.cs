using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour {
    public GameObject telaMain, telaFalha, telaSucesso;

    public TelaVitoriaUI vitoria;

    // Diretriz
    public GameObject diretrizPanel;
    public Text textoDiretriz;
    List<Diretriz> diretrizes = new List<Diretriz>();

    // Missão
    public Button botaoAcao, botaoReiniciarTetris;
    public GameObject missaoPanel;
    public Objetivo objetivo;
    Missao missao;
    

    public Text dinheiro;
    public TextMeshProUGUI timer;

    void Awake() {
        vitoria = transform.GetComponentInChildren<TelaVitoriaUI>(true);
    }

    void Start() {
        diretrizPanel.SetActive(false);

        botaoAcao.gameObject.SetActive(false);
        botaoAcao.onClick.AddListener(delegate { HandleBotaoAcao(); });

        botaoReiniciarTetris.gameObject.SetActive(false); 
        botaoReiniciarTetris.onClick.AddListener(delegate { HandleBotaoRecuperar(); }); 

        missaoPanel.SetActive(false);

        AtualizarDinheiro();
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

            Text titulo = missaoPanel.transform.Find("Titulo").GetComponent<Text>();
            Text descricao = missaoPanel.transform.Find("Descricao").GetComponent<Text>();

            titulo.text = objetivoInicial.missao.info.nome;
            descricao.text = objetivoInicial.missao.info.descricao;
        } else {
            this.objetivo = null;
        }
    }

    // Handle do clique no botão "Fazer Ação"
    [SerializeField]
    void HandleBotaoAcao() {
        objetivo.Concluir();
        MostrarBotaoAcao(null, false);
    }

    // Handle do clique no botão "Iniciar Missão"
    public void HandleBotaoIniciarMissao() {
        objetivo.Concluir();
        MostrarMissaoInfo(null, false);
    }

    public void FalhaMissao(Missao missao) {
        this.missao = missao;
        telaFalha.SetActive(true);
        Time.timeScale = 0;
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

    #region Tetris
    
    public void MostrarBotaoRecuperar(bool mostrar) {
        botaoReiniciarTetris.gameObject.SetActive(mostrar);
    }

    void HandleBotaoRecuperar() {
        Debug.Log("Não esquece de mim! :(");
        // Iniciar estado com cargas caidas...
        Player.instance.RecuperarCargasProximas();
        MostrarBotaoRecuperar(false);
    }

    #endregion

    public void AtualizarDinheiro() {
        dinheiro.text = Player.instance.GetDinheiro().ToString("C2");
    }

    public void MostrarTimer(bool mostrar) {
        timer.gameObject.SetActive(mostrar);
    }

    public void AtualizarTimer(float tempo) {
        int minutos = (int) tempo / 60;
        int segundos = (int) tempo % 60;

        timer.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }
}
