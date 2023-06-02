using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OficinaUI : MonoBehaviour {
    public OficinaController controller;

    public Text nome, descricao, preco;
    public Button comprarButton;

    public GameObject gridPanel, detalhesPanel;
    IUpgrade upgradeSelecionado;

    public void ShowDetalhes(IUpgrade upgrade) {
        upgradeSelecionado = upgrade;

        nome.text = upgrade.nome;
        descricao.text = upgrade.descricao;
        preco.text = upgrade.custo.ToString("C2");

        if (upgrade.comprado) {
            preco.gameObject.SetActive(false);
            comprarButton.interactable = true;
            comprarButton.GetComponentInChildren<Text>().text = upgrade.ativo ? "Desativar" : "Ativar";
        } else {
            preco.gameObject.SetActive(true);
            comprarButton.interactable = Player.instance.GetDinheiro() >= upgrade.custo;
            comprarButton.GetComponentInChildren<Text>().text = "Comprar";
        }

        HandleMostrarDetalhes();
    }

    public void HandleMostrarGrid() {
        gridPanel.SetActive(true);
        detalhesPanel.SetActive(false);
    }

    public void HandleMostrarDetalhes() {
        gridPanel.SetActive(false);
        detalhesPanel.SetActive(true);
    }

    public void HandleComprar() {
        if (!upgradeSelecionado.comprado) {
            upgradeSelecionado.Comprar();
        } else if (upgradeSelecionado.ativo) {
            upgradeSelecionado.Desativar();
        } else {
            upgradeSelecionado.Ativar();
        }
        
        ShowDetalhes(upgradeSelecionado);
    }
}
