using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController instance;

    public Button botaoAcao, botaoIniciarMissao, botaoInterromperMissao, botaoConfirm;
    public Text textoMissaoConcluida;

    Objetivo objetivo;

    void Start() {
        instance = this;

        textoMissaoConcluida.gameObject.SetActive(false);

        botaoAcao.gameObject.SetActive(false);
        botaoAcao.onClick.AddListener(delegate { HandleBotaoAcao();});

        botaoIniciarMissao.gameObject.SetActive(false);
        botaoIniciarMissao.onClick.AddListener(delegate { HandleBotaoIniciarMissao();});

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
    public void PlayerNaAreaDeIniciarMissao(Objetivo objetivo, bool estado) {
        this.objetivo = objetivo;
        botaoIniciarMissao.gameObject.SetActive(estado);
    }

    // Handle do clique no botão "Fazer Ação"
    void HandleBotaoAcao() {
        objetivo.Concluir();
        botaoAcao.gameObject.SetActive(false);
    }

    // Handle do clique no botão "Iniciar Missão"
    void HandleBotaoIniciarMissao() {
        objetivo.Concluir();
        botaoInterromperMissao.gameObject.SetActive(true);
        botaoIniciarMissao.gameObject.SetActive(false);
    }

    // Handle do clique no botão "Interromper Missão"
    void HandleBotaoInterromperMissao() {
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
}
