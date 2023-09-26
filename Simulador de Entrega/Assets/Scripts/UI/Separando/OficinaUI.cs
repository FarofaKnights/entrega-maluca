using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OficinaUI : MonoBehaviour {
    public OficinaController controller;

    public Text nome, descricao, preco;
    public Button comprarButton;

    public GameObject gridPanel, detalhesPanel, upgradesHolder;
    IUpgrade upgradeSelecionado;
    
    public string metodoOrdenacao = "padrao"; // Opções: "padrao", "preco", "nome"

    void Start() {
        int id = 1;

        foreach (Transform item in upgradesHolder.transform) {
            IUpgrade upgrade = item.GetComponent<IUpgrade>();
            if (upgrade != null) {
                upgrade.id = id;
                id++;
            }
        }
    }

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

    #region Ordenacao

    void MergeSort(IUpgrade[] upgrades, int inicio, int fim) {
        if (inicio < fim) {
            int meio = (inicio + fim) / 2;

            MergeSort(upgrades, inicio, meio);
            MergeSort(upgrades, meio + 1, fim);

            Merge(upgrades, inicio, meio, fim);
        }
    }

    void Merge(IUpgrade[] upgrades, int inicio, int meio, int fim) {
        int n1 = meio - inicio + 1;
        int n2 = fim - meio;

        IUpgrade[] L = new IUpgrade[n1];
        IUpgrade[] R = new IUpgrade[n2];

        for (int a = 0; a < n1; a++)
            L[a] = upgrades[inicio + a];

        for (int b = 0; b < n2; b++)
            R[b] = upgrades[meio + 1 + b];

        int i = 0, j = 0, k = inicio;

        while (i < n1 && j < n2) {
            if (Comparacao(L[i], R[j])) {
                upgrades[k] = L[i];
                i++;
            } else {
                upgrades[k] = R[j];
                j++;
            }

            k++;
        }

        while (i < n1) {
            upgrades[k] = L[i];
            i++;
            k++;
        }

        while (j < n2) {
            upgrades[k] = R[j];
            j++;
            k++;
        }
    }

    bool Comparacao(IUpgrade a, IUpgrade b) {
        switch (metodoOrdenacao) {
            case "padrao":
                return a.id < b.id;
            case "preco":
                if (a.custo == b.custo) return a.nome.CompareTo(b.nome) < 0;
                else return a.custo < b.custo;
            case "nome":
                return a.nome.CompareTo(b.nome) < 0;
            default:
                return false;
        }
    }

    public void Ordenar(string metodo) {
        metodoOrdenacao = metodo;

        List<IUpgrade> upgrades = new List<IUpgrade>();

        foreach (Transform item in upgradesHolder.transform) {
            IUpgrade upgrade = item.GetComponent<IUpgrade>();
            if (upgrade != null) {
                upgrades.Add(upgrade);
            }
        }

        IUpgrade[] upgradesArray = upgrades.ToArray();

        MergeSort(upgradesArray, 0, upgradesArray.Length - 1);

        for (int i = 0; i < upgradesArray.Length; i++) {
            upgradesArray[i].transform.SetSiblingIndex(i);
        }
    }

    #endregion

    public void Mostrar() {
        Tela tela = GetComponent<Tela>();
        tela.Mostrar();

        HandleMostrarGrid();
    }

    public void Esconder() {
        Tela tela = GetComponent<Tela>();
        tela.Esconder();
    }
}
