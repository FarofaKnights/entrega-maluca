using UnityEngine;
using UnityEngine.UI;

public class EncaixeUIController : MonoBehaviour {
    public Button botaoConfirm;
    public GameObject tutorialEncaixeMovimento, tutorialEncaixeRotacao;
    bool estaNoEncaixe = false; // Solucao temporaria


    void Start() {
        botaoConfirm.onClick.AddListener(delegate { Confirm(); });
    }

    void FixedUpdate() {
        if (estaNoEncaixe && Cacamba.instance.objSelecionado != null) {
            GameObject obj = Cacamba.instance.objSelecionado;
            Caixas caixa = obj.GetComponent<Caixas>();
            if (caixa != null) {
                bool estaRodando = caixa.rodando;
                tutorialEncaixeMovimento.SetActive(!estaRodando);
                tutorialEncaixeRotacao.SetActive(estaRodando);
            }
        }
    }

    public void Confirm() {
        if (Cacamba.instance.completed) {
            Cacamba.instance.FinalizarTetris();
            Esconder();
        }
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
