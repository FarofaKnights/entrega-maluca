using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalidadeBtn : MonoBehaviour {
    public VisualPlayer.SkinLocation localizacao;
    public Toggle toggle;

    public void HandleSelecionar() {
        if (toggle.isOn) UIController.oficina.SelecionarLocalSkin(localizacao);
    }
}
