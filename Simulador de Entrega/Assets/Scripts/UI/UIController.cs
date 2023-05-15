using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController instance;

    public Button botaoAcao, botaoInterromperMissao, botaoConfirm;
    public Text textoMissaoConcluida, textoDiretriz;

    public GameObject missaoPanel, diretrizPanel;

    Objetivo objetivo;

    List<Diretriz> diretrizes = new List<Diretriz>();

    void Start() {
        instance = this;

        textoMissaoConcluida.gameObject.SetActive(false);

        diretrizPanel.SetActive(false);

        botaoAcao.gameObject.SetActive(false);
        botaoAcao.onClick.AddListener(delegate { HandleBotaoAcao();});

        missaoPanel.SetActive(false);

        botaoInterromperMissao.gameObject.SetActive(false);
        botaoInterromperMissao.onClick.AddListener(delegate { HandleBotaoInterromperMissao();});

        botaoConfirm.gameObject.SetActive(false);
        botaoConfirm.onClick.AddListener(delegate { Confirm(); });

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
        botaoInterromperMissao.gameObject.SetActive(true);
        missaoPanel.SetActive(false);
    }

    // Handle do clique no botão "Interromper Missão"
    public void HandleBotaoInterromperMissao() {
        Player.instance.InterromperMissao();
        StartDrag.sd.Confirm();
        botaoInterromperMissao.gameObject.SetActive(false);
    }

    public void MissaoConcluida() {
        botaoInterromperMissao.gameObject.SetActive(false);
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
        }
    }

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
        foreach (Diretriz d in diretrizes) {
            text += d.texto + "\n";
        }
        textoDiretriz.text = text;
    }
}
