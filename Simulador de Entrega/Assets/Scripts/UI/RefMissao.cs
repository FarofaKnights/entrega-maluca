using UnityEngine;
using UnityEngine.UI;

public class RefMissao : MonoBehaviour {
    public Missao missao;

    public Color corFinalizada;

    void Start() {
        if (missao == null) return;
        // Suspenso até próxima atualização

        /*
        Image image = GetComponent<Image>();

        if (missao.FoiFinalizada()) {
            BotaoToggle botaoToggle = GetComponent<BotaoToggle>();
            image.color = corFinalizada;
            botaoToggle.UpdateValues();
        }
        */ 
    }
}
