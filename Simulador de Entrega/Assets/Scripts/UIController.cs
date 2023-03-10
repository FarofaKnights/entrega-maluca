using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController instance;

    public Button botaoEntrega, botaoReceber, botaoIniciarMissao, botaoInterromperMissao;
    Destino destino;

    void Start() {
        instance = this;

        botaoEntrega.gameObject.SetActive(false);
        botaoEntrega.onClick.AddListener(delegate { HandleBotaoEntregar();});
        botaoReceber.gameObject.SetActive(false);
        botaoReceber.onClick.AddListener(delegate { HandleBotaoReceber();});
        botaoIniciarMissao.gameObject.SetActive(false);
        botaoIniciarMissao.onClick.AddListener(delegate { HandleBotaoIniciarMissao();});
        botaoInterromperMissao.onClick.AddListener(delegate { HandleBotaoInterromperMissao();});

    }

    // Chamado quando player entra/sai em uma área de Entregar Carga
    public void PlayerNaAreaDeEntrega(Destino destino, bool estado) {
        this.destino = destino;
        botaoEntrega.gameObject.SetActive(estado);
    }

    // Chamado quando player entra/sai em uma área de Receber Carga
    public void PlayerNaAreaDeReceber(Destino destino, bool estado) {
        this.destino = destino;
        botaoReceber.gameObject.SetActive(estado);
    }

    // Chamado quando player entra/sai em uma área de Iniciar Missão
    public void PlayerNaAreaDeIniciarMissao(Destino destino, bool estado) {
        this.destino = destino;
        botaoIniciarMissao.gameObject.SetActive(estado);
    }

    // Handle do clique no botão "Receber Carga"
    void HandleBotaoReceber() {
        destino.Concluir();
    }

    // Handle do clique no botão "Entregar Carga"
    void HandleBotaoEntregar() {
        destino.Concluir();
    }

    // Handle do clique no botão "Iniciar Missão"
    void HandleBotaoIniciarMissao() {
        destino.Concluir();
    }

    // Handle do clique no botão "Interromper Missão"
    void HandleBotaoInterromperMissao() {
        Player.instance.InterromperMissao();
    }
}
