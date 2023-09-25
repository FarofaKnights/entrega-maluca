using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    public GameObject telaMain, telaFalha, telaSucesso;

    // Diretriz
    public GameObject diretrizPanel;
    public Text textoDiretriz;
    List<Diretriz> diretrizes = new List<Diretriz>();

    // Missão
    public GameObject textoMissaoConcluida;
    public GameObject missaoPanel;
    Objetivo objetivo;
    

    public Text dinheiro;
    public TextMeshProUGUI timer;

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
            this.objetivo = objetivo;

            Text titulo = missaoPanel.transform.Find("Titulo").GetComponent<Text>();
            Text descricao = missaoPanel.transform.Find("Descricao").GetComponent<Text>();

            titulo.text = objetivo.missao.titulo;
            descricao.text = objetivo.missao.descricao;
        } else {
            this.objetivo = null;
        }
    }

    // Handle do clique no botão "Fazer Ação"
    [SerializeField]
    void HandleBotaoAcao() {
        objetivo.Concluir();
    }

    // Handle do clique no botão "Iniciar Missão"
    public void HandleBotaoIniciarMissao() {
        objetivo.Concluir();
        missaoPanel.SetActive(false);
        refMinimapaPanel.SetActive(false);
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
