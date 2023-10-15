using UnityEngine;
using UnityEngine.UI;

public class EncaixeUIController : MonoBehaviour {
    public Button botaoConfirm;
    public GameObject tutorialEncaixeMovimento, tutorialEncaixeRotacao;
    bool estaNoEncaixe = false; // Solucao temporaria
    Caixas[] c;


    void Start() {
        botaoConfirm.onClick.AddListener(delegate { Confirm(); });
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

    public void SetCargas(Caixas[] c) {
        this.c = c;
    }

    public void Confirm() {
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
        estaNoEncaixe = true;
    }

    public void Esconder() {
        Tela tela = GetComponent<Tela>();
        tela.Esconder();
        estaNoEncaixe = false;
    }
}
