using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController instance;

    public Button botaoAcao, botaoConfirm;
    public Text textoMissaoConcluida, textoDiretriz;

    public GameObject missaoPanel, diretrizPanel;
    public GameObject refMissaoPanel, refEncaixePanel, refOficinaPanel, refPausaPanel, refMinimapaPanel;

    public Text dinheiro;

    Objetivo objetivo;

    List<Diretriz> diretrizes = new List<Diretriz>();

    void Start() {
        instance = this;

        textoMissaoConcluida.gameObject.SetActive(false);

        diretrizPanel.SetActive(false);

        botaoAcao.gameObject.SetActive(false);
        botaoAcao.onClick.AddListener(delegate { HandleBotaoAcao();});

        missaoPanel.SetActive(false);

        botaoConfirm.gameObject.SetActive(false);
        botaoConfirm.onClick.AddListener(delegate { Confirm(); });

        AtualizarDinheiro();
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
        textoMissaoConcluida.gameObject.SetActive(true);

        // Espera 2 segundos e esconde o texto
        StartCoroutine(EsconderTextoMissaoConcluida());
    }

    IEnumerator EsconderTextoMissaoConcluida() {
        yield return new WaitForSeconds(2);
        textoMissaoConcluida.gameObject.SetActive(false);
    }
    public void Confirm()
    {
        if (StartDrag.sd.completed)
        {
            StartDrag.sd.Confirm();
            objetivo.Finalizar();
            botaoConfirm.gameObject.SetActive(false);
            refMinimapaPanel.SetActive(true);
        }
    }

    public void InterromperTetris() {
        // Solução temporária para o caos do startdrag
        StartDrag.sd.Confirm();
        botaoConfirm.gameObject.SetActive(false);
        refMinimapaPanel.SetActive(true);
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
}
