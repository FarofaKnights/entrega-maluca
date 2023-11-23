
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface FormatadorDeValor {
    public string Formatar(float valor);
}

public class CampoNomeValor : MonoBehaviour {
    public Text nome, valor;
    public CampoNomeValorScriptable info;
    public enum TipoFormato {
        Padrao,
        Porcentagem,
        Dinheiro,
        Tempo
    }

    public TipoFormato tipoFormato = TipoFormato.Padrao;

    public class FormatoPadrao: FormatadorDeValor {
        public string Formatar(float valor) {
            return valor.ToString();
        }
    }

    public class FormatoPorcentagem: FormatadorDeValor {
        public string Formatar(float valor) {
            return valor.ToString("P");
        }
    }

    public class FormatoDinheiro: FormatadorDeValor {
        public string Formatar(float valor) {
            return valor.ToString("C2");
        }
    }

    public class FormatoTempo: FormatadorDeValor {
        public string Formatar(float valor) {
            return valor.ToString("0.00") + "s";
        }
    }

    FormatadorDeValor GetFormatador() {
        switch (tipoFormato) {
            case TipoFormato.Padrao:
                return new FormatoPadrao();
            case TipoFormato.Porcentagem:
                return new FormatoPorcentagem();
            case TipoFormato.Dinheiro:
                return new FormatoDinheiro();
            case TipoFormato.Tempo:
                return new FormatoTempo();
            default:
                return new FormatoPadrao();
        }
    }

    public void SetNome(string nome) {
        this.nome.text = nome;
    }

    public void SetValor(string valor) {
        this.valor.text = valor;
    }

    public void SetDados(string nome, string valor) {
        SetNome(nome);
        SetValor(valor);
    }

    public void SetAnimatedValor(float valorInicial, float valorFinal) {
        StartCoroutine(SetAnimatedValorCoroutine(valorInicial, valorFinal));
    }

    public void HideValues() {
        nome.gameObject.SetActive(false);
        valor.gameObject.SetActive(false);
    }

    public void ShowValues() {
        nome.gameObject.SetActive(true);
        valor.gameObject.SetActive(true);
    }

    public IEnumerator ScaleDownAnimation() {
        float duracao = info.tempoFadeIn;
        Vector3 scaleInicial = transform.localScale * 1.25f;
        Vector3 scaleFinal = transform.localScale;

        ShowValues();

        return Animations.UIScaleDown(gameObject, duracao, scaleInicial, scaleFinal, false);
    }

    public IEnumerator SetAnimatedValorCoroutine(float valorInicial, float valorFinal) {
        FormatadorDeValor format = GetFormatador();
        float duracao = info.tempoShowValor;
        float tempo = 0;

        while (tempo < duracao) {
            tempo += Time.unscaledDeltaTime;
            float valor = Mathf.Lerp(valorInicial, valorFinal, tempo / duracao);
            if (format == null) SetValor(valor.ToString());
            else SetValor(format.Formatar(valor));
            yield return null;
        }
    }

    public IEnumerator JumpValueCourotine() {
        float duracao = info.tempoJumpValor / 2;
        float tempo = 0;

        Vector3 scaleInicial = valor.transform.localScale;
        Vector3 scaleFinal = scaleInicial * info.scaleMultJump;

        while (tempo < duracao) {
            tempo += Time.unscaledDeltaTime;
            valor.transform.localScale = Vector3.Lerp(scaleInicial, scaleFinal, tempo / duracao);
            yield return null;
        }

        tempo = 0;
        while (tempo < duracao) {
            tempo += Time.unscaledDeltaTime;
            valor.transform.localScale = Vector3.Lerp(scaleFinal, scaleInicial, tempo / duracao);
            yield return null;
        }
    }


    public virtual IEnumerator ShowValueAnimation(float valorInicial, float valorFinal) {
        yield return StartCoroutine(ScaleDownAnimation());
        yield return StartCoroutine(SetAnimatedValorCoroutine(valorInicial,valorFinal));
        yield return StartCoroutine(JumpValueCourotine());
    }
}
