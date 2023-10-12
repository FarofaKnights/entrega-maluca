using UnityEngine;
using UnityEngine.UI;

public class EncaixeUIController : MonoBehaviour {
    public Button botaoConfirm, botaoReiniciarTetris;
    public GameObject tutorialEncaixeMovimento, tutorialEncaixeRotacao;
    bool estaNoEncaixe = false; // Solucao temporaria


    void Start() {
        botaoConfirm.onClick.AddListener(delegate { Confirm(Cacamba.instance.cargas); });

        botaoReiniciarTetris.gameObject.SetActive(false); 
        botaoReiniciarTetris.onClick.AddListener(delegate { Cacamba.instance.ReiniciarTetris(); }); 
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

    public void Confirm(Caixas[] c) {
        Cacamba.instance.MudarCaixas(c); 
    }

    public void InterromperTetris() {
        // Solução temporária para o caos do startdrag
        Cacamba.instance.FinalizarTetris();
        Esconder();
    }

    public void Mostrar() {
        Tela tela = GetComponent<Tela>();
        tela.Mostrar();
    }

    public void Esconder() {
        Tela tela = GetComponent<Tela>();
        tela.Esconder();
    }
}
