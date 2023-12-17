using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CargaRelatorioUI : CampoNomeValor {
    public Text valorTxt;
    public RectTransform traco;
    public float targetWidth = 275f;


    public IEnumerator ShowValueAnimation(string nome, float porcentagem, float valor, bool entregue) {
        this.nome.text = nome;
        valorTxt.gameObject.SetActive(false);
        // change width to 0 and maintain height
        traco.sizeDelta = new Vector2(0, traco.sizeDelta.y);
        traco.gameObject.SetActive(false);

        yield return StartCoroutine(ScaleDownAnimation());
        if (porcentagem != 1) yield return StartCoroutine(SetAnimatedValorCoroutine(1,porcentagem));
        if (entregue) {
            yield return StartCoroutine(JumpValueCourotine());
            valorTxt.gameObject.SetActive(true);
            valorTxt.text = valor.ToString("C2");
        } else {
            traco.gameObject.SetActive(true);

            yield return StartCoroutine(Animations.UnscaledAnimation(0.25f, (float t) => {
                traco.sizeDelta = new Vector2(Mathf.Lerp(0, targetWidth, t), traco.sizeDelta.y);
            }));

            valorTxt.gameObject.SetActive(true);
            valorTxt.text = (porcentagem == 0) ? "Destruída" : "Perdida";
        }
    }

    public IEnumerator ShowValueAnimation(StatusCarga status) {
        return ShowValueAnimation(status.nome, status.porcentagem, status.valor, status.entregue);
    }
}
