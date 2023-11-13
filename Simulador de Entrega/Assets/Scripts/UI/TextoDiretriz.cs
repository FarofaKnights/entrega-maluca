using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextoDiretriz : MonoBehaviour {
    private Text _text;

    public Text text {
        get {
            if (_text == null) _text = GetComponentsInChildren<Text>()[0];
            return _text;
        }
    }

    Diretriz diretriz;

    public Color normalColor;
    public Color destaqueColor;

    public bool riscado = false, destaque = true;
    public GameObject risco, tab;

    void Start() {
        SetRiscado(riscado);
        SetDestaque(destaque);
    }

    void SetNivel(int nivel) {
        TextoDiretrizInfo info;

        if (nivel == 1) info = UIController.diretriz.infoNivel1;
        else info = UIController.diretriz.infoNivel2;

        text.fontSize = info.tamanhoFonte;
        if (info.tamanhoTab == 0) tab.SetActive(false);
        else {
            tab.SetActive(true);
            tab.GetComponent<RectTransform>().sizeDelta = new Vector2(info.tamanhoTab, 1);
        }
    }

    public void SetRiscado(bool riscado) {
        this.riscado = riscado;
        risco.SetActive(riscado);
        if (riscado) SetDestaque(false);
    }

    public void SetDestaque(bool destaque) {
        this.destaque = destaque;

        if (destaque) {
            text.color = destaqueColor;
        } else {
            text.color = normalColor;
        }
    }

    public void SetDiretriz(Diretriz diretriz) {
        this.diretriz = diretriz;
        SetNivel(diretriz.GetNivel());

        string texto = diretriz.texto;

        if (diretriz.GetNivel() > 1) texto = "▪ " + texto;

        text.text = texto;
    }

    public Diretriz GetDiretriz() {
        return diretriz;
    }
}
