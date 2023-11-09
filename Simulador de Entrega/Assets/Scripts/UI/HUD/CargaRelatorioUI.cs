using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CargaRelatorioUI : CampoNomeValor {
    public Text valorTxt;

    public IEnumerator ShowValueAnimation(string nome, float porcentagem, float valor) {
        this.nome.text = nome;
        yield return StartCoroutine(ScaleDownAnimation());
        if (porcentagem != 1) yield return StartCoroutine(SetAnimatedValorCoroutine(1,porcentagem));
        yield return StartCoroutine(JumpValueCourotine());
        valorTxt.text = valor.ToString("C2");
    }

    public IEnumerator ShowValueAnimation(StatusCarga status) {
        return ShowValueAnimation(status.nome, status.porcentagem, status.valor);
    }
}
