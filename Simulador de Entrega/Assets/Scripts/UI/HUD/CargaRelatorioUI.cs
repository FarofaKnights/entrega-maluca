using UnityEngine;
using UnityEngine.UI;

public class CargaRelatorioUI : MonoBehaviour {
    public Text nomeTxt, porcentagemTxt, valorTxt;

    public void AtualizarValores(StatusCarga status) {
        Debug.Log("Atualizando valores");
        nomeTxt.text = status.nome;
        porcentagemTxt.text = status.porcentagem.ToString("0.00%");
        valorTxt.text = status.valor.ToString("C2");
    }
}
