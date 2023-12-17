using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelaVitoriaUI : MonoBehaviour {
    public GameObject textoMissaoConcluida;
    public GameObject relatorio;

    public GameObject tentarNovamenteBtn, concluirBtn;
    public CampoNomeValor dinheiroFixoCampo, tempoCampo, cabecalhoCampo;
    public GameObject holderCargas, cargasPrefab;
    public GameObject[] avaliacaoSelecionada;

    Missao missaoVitoriosa;
    StatusMissao status;

    public void MissaoConcluida(Missao missao, StatusMissao status) {
        missaoVitoriosa = missao;
        this.status = status;
        relatorio.SetActive(true);
        GerarRelatorio();
        Time.timeScale = 0;
    }

    IEnumerator EsconderTextoMissaoConcluida() {
        yield return new WaitForSeconds(1);
        textoMissaoConcluida.SetActive(false);
        gameObject.SetActive(false);
    }

    public void GerarRelatorio() {
        StartCoroutine(GerarRelatorioCoroutine());
    }

    IEnumerator GerarRelatorioCoroutine() {
        ClearAvaliacao();
        tempoCampo.HideValues();
        dinheiroFixoCampo.HideValues();
        cabecalhoCampo.HideValues();

        foreach (Transform child in holderCargas.transform) {
            Destroy(child.gameObject);
        }
        
        if (status.tempo < 0) {
            tempoCampo.SetValor("Não se aplica");
        } else yield return StartCoroutine(tempoCampo.ShowValueAnimation(0, status.tempo));


        yield return StartCoroutine(dinheiroFixoCampo.ShowValueAnimation(0, status.dinheiro));
        yield return StartCoroutine(cabecalhoCampo.ScaleDownAnimation());

        foreach (StatusCarga statusCarga in status.cargas) {
            GameObject go = Instantiate(cargasPrefab, holderCargas.transform);
            yield return StartCoroutine(go.GetComponent<CargaRelatorioUI>().ShowValueAnimation(statusCarga));
        }

        concluirBtn.SetActive(status.avaliacao > 1); // Caso avaliação menor que 1, não mostra botão de concluir

        SelecionarAvaliacao(status.avaliacao);
    }

    void SelecionarAvaliacao(int avaliacao) {
        for (int i = 0; i < avaliacaoSelecionada.Length; i++) {
            avaliacaoSelecionada[i].SetActive(i == avaliacao - 1);
        }
    }

    void ClearAvaliacao() {
        for (int i = 0; i < avaliacaoSelecionada.Length; i++) {
            avaliacaoSelecionada[i].SetActive(false);
        }
    }

    public void HandleCloseRelatorio() {
        relatorio.SetActive(false);
        textoMissaoConcluida.SetActive(true);
        Time.timeScale = 1;

        // Espera 2 segundos e esconde o texto
        StartCoroutine(EsconderTextoMissaoConcluida());
    }

    public void HandleReiniciar() {
        Time.timeScale = 1;
        MissaoManager.instance.ReiniciarMissao(missaoVitoriosa);
        relatorio.SetActive(false);
        gameObject.SetActive(false);
    }
}
