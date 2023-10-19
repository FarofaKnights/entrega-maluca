using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelaVitoriaUI : MonoBehaviour {
    public GameObject textoMissaoConcluida;
    public GameObject relatorio;

    public GameObject tentarNovamenteBtn, concluirBtn;
    public Text dinheiroFixoTxt, tempoTxt;
    public GameObject holderCargas, cargasPrefab;
    public GameObject[] avaliacaoSelecionada;

    Missao missaoVitoriosa;
    StatusMissao status;

    public void MissaoConcluida(Missao missao, StatusMissao status) {
        missaoVitoriosa = missao;
        this.status = status;
        relatorio.SetActive(true);
        GerarRelatorio();
    }

    IEnumerator EsconderTextoMissaoConcluida() {
        yield return new WaitForSeconds(1);
        textoMissaoConcluida.SetActive(false);
        gameObject.SetActive(false);
    }

    public void GerarRelatorio() {
        dinheiroFixoTxt.text = status.dinheiro.ToString("C2");
        tempoTxt.text = status.tempo.ToString("0.00") + "s";

        foreach (Transform child in holderCargas.transform) {
            Destroy(child.gameObject);
        }

        foreach (StatusCarga statusCarga in status.cargas) {
            GameObject go = Instantiate(cargasPrefab, holderCargas.transform);
            go.GetComponent<CargaRelatorioUI>().AtualizarValores(statusCarga);
        }

        concluirBtn.SetActive(status.avaliacao > 1); // Caso avaliação menor que 1, não mostra botão de concluir

        SelecionarAvaliacao(status.avaliacao);
    }

    void SelecionarAvaliacao(int avaliacao) {
        for (int i = 0; i < avaliacaoSelecionada.Length; i++) {
            avaliacaoSelecionada[i].SetActive(i == avaliacao - 1);
        }
    }

    public void HandleCloseRelatorio() {
        relatorio.SetActive(false);
        textoMissaoConcluida.SetActive(true);

        // Espera 2 segundos e esconde o texto
        StartCoroutine(EsconderTextoMissaoConcluida());
    }

    public void HandleReiniciar() {
        missaoVitoriosa.Resetar();
        relatorio.SetActive(false);
        gameObject.SetActive(false);
    }
}
