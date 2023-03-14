using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController instance;

    public Button botaoAcao, botaoIniciarMissao, botaoInterromperMissao;
    public Text textoMissaoConcluida;

    Destino destino;

    void Start() {
        instance = this;

        textoMissaoConcluida.gameObject.SetActive(false);

        botaoAcao.gameObject.SetActive(false);
        botaoAcao.onClick.AddListener(delegate { HandleBotaoAcao();});

        botaoIniciarMissao.gameObject.SetActive(false);
        botaoIniciarMissao.onClick.AddListener(delegate { HandleBotaoIniciarMissao();});

        botaoInterromperMissao.gameObject.SetActive(false);
        botaoInterromperMissao.onClick.AddListener(delegate { HandleBotaoInterromperMissao();});

    }

    // Chamado quando player entra/sai em uma área de Acao
    public void PlayerNaAreaDeAcao(Destino destino, bool estado) {
        this.destino = destino;
        botaoAcao.gameObject.SetActive(estado);
    }

    // Chamado quando player entra/sai em uma área de Iniciar Missão
    public void PlayerNaAreaDeIniciarMissao(Destino destino, bool estado) {
        this.destino = destino;
        botaoIniciarMissao.gameObject.SetActive(estado);
    }

    // Handle do clique no botão "Fazer Ação"
    void HandleBotaoAcao() {
        destino.Concluir();
        botaoAcao.gameObject.SetActive(false);
    }

    // Handle do clique no botão "Iniciar Missão"
    void HandleBotaoIniciarMissao() {
        destino.Concluir();
        botaoInterromperMissao.gameObject.SetActive(true);
        botaoIniciarMissao.gameObject.SetActive(false);
    }

    // Handle do clique no botão "Interromper Missão"
    void HandleBotaoInterromperMissao() {
        Player.instance.InterromperMissao();
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
}
