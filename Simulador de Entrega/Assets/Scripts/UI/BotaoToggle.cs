using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotaoToggle : MonoBehaviour {

    Toggle toggle;
    Color corPadrao, corAtiva;

    void Start()  {
        HandleChange();
    }

    void UpdateValues() {
        toggle = GetComponent<Toggle>();
        corPadrao = toggle.colors.normalColor;
        corAtiva = toggle.colors.pressedColor;
    }

    public void HandleChange() {
        if (toggle == null) UpdateValues();

        Color cor = toggle.isOn ? corAtiva : corPadrao;
        toggle.GetComponent<Image>().color = cor;
    }
}
