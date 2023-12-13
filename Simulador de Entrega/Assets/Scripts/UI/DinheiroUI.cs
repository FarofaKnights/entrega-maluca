using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DinheiroUI : MonoBehaviour {
    public TextMeshProUGUI dinheiro;

    public void AtualizarDinheiro() {
        dinheiro.text = Player.instance.GetDinheiro().ToString("C2");
    }
}
