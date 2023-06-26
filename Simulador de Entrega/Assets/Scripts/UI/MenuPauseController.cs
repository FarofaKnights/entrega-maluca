using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPauseController : MonoBehaviour {
    public GameObject missaoPanel, menuPanel, opcoesPanel;
    public GameObject missaoDetails, missaoList, missaoItemPrefab;

    public GameObject interromperMissaoBtn;
    public GameObject missaoJaFoiConcluida, missaoAtual;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
    }

    public void OpenMissao() {
        missaoPanel.SetActive(true);
        opcoesPanel.SetActive(false);
        menuPanel.SetActive(false);
        GenerateMissaoList();
    }

    public void OpenMenu() {
        missaoPanel.SetActive(false);
        opcoesPanel.SetActive(false);
        menuPanel.SetActive(true);
        
        UpdateBotaoInterromperMissao();
    }

    public void OpenOpcoes() {
        missaoPanel.SetActive(false);
        opcoesPanel.SetActive(true);
        menuPanel.SetActive(false);

        UIController.instance.UpdateSliders();
    }

    void UpdateBotaoInterromperMissao() {
        // Se jogador estiver em uma missão, mostra o botão de interromper
        interromperMissaoBtn.SetActive(Player.instance.missaoAtual != null);

        // Por algum motivo a unity não atualiza o layout automaticamente nesse caso, esta é a solução que encontrei
        Canvas.ForceUpdateCanvases();
        interromperMissaoBtn.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;
        interromperMissaoBtn.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
    }

    public void HandleBotaoInterromperMissao() {
        Player.instance.InterromperMissao();
        StartDrag.sd.Confirm();

        UpdateBotaoInterromperMissao();
    }

    GameObject GenerateMissaoItem(Missao missao) {
        ToggleGroup toggleGroup = missaoList.GetComponent<ToggleGroup>();

        GameObject missaoItem = Instantiate(missaoItemPrefab, missaoList.transform);
        missaoItem.GetComponent<RefMissao>().missao = missao;
        missaoItem.GetComponentInChildren<Text>().text = missao.titulo;

        Toggle toggle = missaoItem.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener(delegate { HandleMissaoItemClick(toggle); });

        return missaoItem;
    }

    public void GenerateMissaoList() {
        List<Missao> missoes = GameManager.instance.missoesDisponiveis;
        List<Missao> missoesConcluidas = GameManager.instance.missoesConcluidas;

        

        // Limpa a lista
        foreach (Transform child in missaoList.transform) {
            Destroy(child.gameObject);
        }

        // Gera a lista
        foreach (Missao missao in missoes) {
            GenerateMissaoItem(missao);
        }

        // Gera a lista de missões concluidas
        foreach (Missao missao in missoesConcluidas) {
            GenerateMissaoItem(missao);
        }
    }

    public void HandleMissaoItemClick(Toggle toggle) {
        RefMissao refMissao = toggle.GetComponent<RefMissao>();
        Missao missao = refMissao.missao;

        if (toggle.isOn) UpdateMissaoDetails(missao);
        else UpdateMissaoDetails(null);
    }

    public void UpdateMissaoDetails(Missao missao) {
        if (missao == null) {
            missaoDetails.SetActive(false);
            return;
        }

        missaoDetails.SetActive(true);

        Text titulo = missaoDetails.transform.Find("Conteudo").Find("Titulo").GetComponent<Text>();
        Text descricao = missaoDetails.transform.Find("Conteudo").Find("Descricao").GetComponent<Text>();

        missaoJaFoiConcluida.SetActive(missao.FoiFinalizada());
        missaoAtual.SetActive(missao == Player.instance.missaoAtual);

        titulo.text = missao.titulo;
        descricao.text = missao.descricao;
    }
}
