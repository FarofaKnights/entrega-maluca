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

    Missao missaoVitoriosa;
    StatusMissao status;

    public void MissaoConcluida(Missao missao, StatusMissao status) {
        missaoVitoriosa = missao;
        this.status = status;
        textoMissaoConcluida.SetActive(true);

        // Espera 2 segundos e esconde o texto
        StartCoroutine(EsconderTextoMissaoConcluida());
    }

    IEnumerator EsconderTextoMissaoConcluida() {
        yield return new WaitForSeconds(1);
        textoMissaoConcluida.SetActive(false);
        relatorio.SetActive(true);
        GerarRelatorio();
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
    }

    public void HandleCloseRelatorio() {
        relatorio.SetActive(false);
        gameObject.SetActive(false);
    }

    public void HandleReiniciar() {
        MissaoManager.instance.ReiniciarMissao();
        relatorio.SetActive(false);
        gameObject.SetActive(false);
    }
}
