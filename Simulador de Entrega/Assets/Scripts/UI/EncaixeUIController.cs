using UnityEngine;
using UnityEngine.UI;

public class EncaixeUIController : MonoBehaviour {
    public Button botaoConfirm;
    public GameObject tutorialEncaixeMovimento;

    public Text wasdText, rotacionarText;
    public Tela tela;


    void Start() {
        botaoConfirm.onClick.AddListener(delegate { Confirm(); });

        Player.instance.onStateChange += state => {
            if (state.GetType() == typeof(EncaixeState)) {
                EncaixeState encaixeState = (EncaixeState) state;
                encaixeState.onRotateChange += CheckRotate;
            }
        };

        if (UIController.HUD.gameObject.activeSelf) {
            Debug.Log("HUD está ligado logo eu vou desligar ele!!!!");
            UIController.HUD.gameObject.SetActive(false);
        }
    }

    void CheckRotate(bool estaRodando) {
        if (estaRodando) {
            wasdText.text = "Rotacionar";
            rotacionarText.text = "Desativar rotação";
        } else {
            wasdText.text = "Mover";
            rotacionarText.text = "Ativar rotação";
        }
    }

    public void Confirm() {
        IState estado = Player.instance.GetState();

        if (estado.GetType() == typeof(EncaixeState)) {
            EncaixeState encaixeState = (EncaixeState) estado;
            encaixeState.CheckConcluido();
        }
    }

    public void InterromperTetris() {
        Player.instance.SetDirigindo();
        Esconder();
    }

    public void Mostrar() {
        tela.Mostrar();
        tela.GetVizinho("HUD")?.Esconder();
    }

    public void Esconder() {
        tela.Esconder();
        tela.GetVizinho("HUD")?.Mostrar();
    }
}
