using UnityEngine;
using UnityEngine.UI;

public class EncaixeUIController : MonoBehaviour {
    public Button botaoConfirm;
    public GameObject tutorialEncaixeMovimento;

    public Text wasdText, rotacionarText;
    public Tela tela;


    void Start() {
        botaoConfirm.onClick.AddListener(delegate { Confirm(); });
    }

    void FixedUpdate() {
        if (tela.visivel && Cacamba.instance.caixaAtual != null) {
            Caixas caixa = Cacamba.instance.caixaAtual;
            if (caixa != null) {
                bool estaRodando = caixa.rodando;
                if (estaRodando) {
                    wasdText.text = "Rotacionar";
                    rotacionarText.text = "Desativar rotação";
                } else {
                    wasdText.text = "Mover";
                    rotacionarText.text = "Ativar rotação";
                }
            }
        }
    }

    public void Confirm() {
        Cacamba.instance.MudarCaixas(); 
    }

    public void InterromperTetris() {
        // Solução temporária para o caos do startdrag
        Cacamba.instance.FinalizarTetris();
        Esconder();
    }

    public void Mostrar() {
        tela.Mostrar();
    }

    public void Esconder() {
        tela.Esconder();
        tela.GetVizinho("HUD")?.Mostrar();
    }
}
