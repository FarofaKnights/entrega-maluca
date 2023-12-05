using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OficinaUI : MonoBehaviour {
    public OficinaController controller;

    public Text descricao;
    public Button comprarButton;

    public TextMeshProUGUI nome, preco, comprarBtnText;

    public GameObject gridPanel, detalhesPanel, upgradesHolder;
    UpgradeObject upgradeSelecionado;
    
    public string metodoOrdenacao = "padrao"; // Opções: "padrao", "preco", "nome"

    public GameObject caracteristicasHolder;
    public GameObject caracteristicaPrefab;

    void Start() {
        int id = 1;

        foreach (Transform item in upgradesHolder.transform) {
            UpgradeButton upgrade = item.GetComponent<UpgradeButton>();
            if (upgrade != null) {
                upgrade.id = id;
                id++;
            }
        }
    }

    public void ShowDetalhes(UpgradeButton upgrade) {
        ShowDetalhes(upgrade.upgradeObject);
    }

    public void ShowDetalhes(UpgradeObject upgrade) {
        upgradeSelecionado = upgrade;

        nome.text = upgrade.nome;
        descricao.text = upgrade.descricao;
        preco.text = upgrade.custo.ToString("C2");

        if (upgrade.comprado) {
            comprarButton.interactable = true;
            comprarBtnText.text = upgrade.ativo ? "Desativar" : "Ativar";
        } else {
            comprarButton.interactable = Player.instance.GetDinheiro() >= upgrade.custo;
            comprarBtnText.text = "Comprar";
        }

        GerarCaracteristicas(upgrade);
        HandleMostrarDetalhes();
    }

    void ReloadLayout(GameObject uiElement) {
        LayoutRebuilder.MarkLayoutForRebuild((RectTransform)uiElement.transform);
        LayoutGroup[] parentLayoutGroups = uiElement.gameObject.GetComponentsInParent<LayoutGroup>();
        foreach (LayoutGroup group in parentLayoutGroups) {
        LayoutRebuilder.MarkLayoutForRebuild((RectTransform)group.transform);
        }
    }

    void GerarCaracteristicas(UpgradeObject upgrade) {
        Dictionary<string, (string, float)> info = upgrade.GetInfo();

        if (info == null || info.Count == 0) {
            caracteristicasHolder.SetActive(false);
            return;
        }

        UpgradeObject upgradeAtual = OficinaController.instance.CurrentOfSameType(upgrade);
        if (!upgrade.exclusivo) upgradeAtual = null;
        else if (upgradeAtual == upgrade) upgradeAtual = null;

        caracteristicasHolder.SetActive(true);

        foreach (Transform item in caracteristicasHolder.transform) {
            Destroy(item.gameObject);
        }

        foreach (KeyValuePair<string, (string, float)> caracteristica in info) {
            GameObject caracteristicaObject = Instantiate(caracteristicaPrefab, caracteristicasHolder.transform);
            caracteristicaObject.transform.Find("Nome").GetComponent<Text>().text = caracteristica.Key;

            if (upgradeAtual == null)
                caracteristicaObject.transform.Find("Valor").GetComponent<Text>().text = caracteristica.Value.Item1;
            else {
                float valorAtual = upgradeAtual.GetInfo()[caracteristica.Key].Item2;
                float valorNovo = caracteristica.Value.Item2;

                string valorAtualTxt = upgradeAtual.GetInfo()[caracteristica.Key].Item1;
                string valorNovoTxt = caracteristica.Value.Item1;

                if (valorAtual > valorNovo) {
                    valorNovoTxt = "<color=red>" + valorNovoTxt + "</color>";
                } else if (valorAtual < valorNovo) {
                    valorNovoTxt = "<color=#23FF00FF>" + valorNovoTxt + "</color>";
                }

                caracteristicaObject.transform.Find("Valor").GetComponent<Text>().text = valorAtualTxt + " -> " + valorNovoTxt;
            }
        }
    }

    public void HandleMostrarGrid() {
        gridPanel.SetActive(true);
        detalhesPanel.SetActive(false);
    }

    public void HandleMostrarDetalhes() {
        gridPanel.SetActive(false);
        detalhesPanel.SetActive(true);
        
        StartCoroutine(UpdateLayoutGroup());
    }

    IEnumerator UpdateLayoutGroup() {
        descricao.transform.parent.gameObject.GetComponent<LayoutGroup>().enabled = false;
        yield return new WaitForEndOfFrame();
        descricao.transform.parent.gameObject.GetComponent<LayoutGroup>().enabled = true;
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

    void MergeSort(UpgradeButton[] upgrades, int inicio, int fim) {
        if (inicio < fim) {
            int meio = (inicio + fim) / 2;

            MergeSort(upgrades, inicio, meio);
            MergeSort(upgrades, meio + 1, fim);

            Merge(upgrades, inicio, meio, fim);
        }
    }

    void Merge(UpgradeButton[] upgrades, int inicio, int meio, int fim) {
        int n1 = meio - inicio + 1;
        int n2 = fim - meio;

        UpgradeButton[] L = new UpgradeButton[n1];
        UpgradeButton[] R = new UpgradeButton[n2];

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

    bool Comparacao(UpgradeButton a, UpgradeButton b) {
        switch (metodoOrdenacao) {
            case "padrao":
                return a.id < b.id;
            case "preco":
                if (a.upgradeObject.custo == b.upgradeObject.custo) return a.upgradeObject.nome.CompareTo(b.upgradeObject.nome) < 0;
                else return a.upgradeObject.custo < b.upgradeObject.custo;
            case "nome":
                return a.upgradeObject.nome.CompareTo(b.upgradeObject.nome) < 0;
            default:
                return false;
        }
    }

    public void Ordenar(string metodo) {
        metodoOrdenacao = metodo;

        List<UpgradeButton> upgrades = new List<UpgradeButton>();

        foreach (Transform item in upgradesHolder.transform) {
            UpgradeButton upgrade = item.GetComponent<UpgradeButton>();
            if (upgrade != null) {
                upgrades.Add(upgrade);
            }
        }

        UpgradeButton[] upgradesArray = upgrades.ToArray();

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

        tela.GetVizinho("HUD")?.Mostrar();
    }
}
