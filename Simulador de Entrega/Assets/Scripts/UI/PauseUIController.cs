using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PauseUIController : MonoBehaviour {
    public GameObject missaoPanel, menuPanel, opcoesPanel;
    public GameObject missaoDetails, missaoList, missaoItemPrefab;

    public GameObject interromperMissaoBtn;
    public GameObject missaoJaFoiConcluida, missaoAtual;

    public AudioMixer audioMixer;
    public Slider efeitoSlider, carroSlider, geralSlider;

    public GameObject missaoSelecionadaRelatorio;
    public GameObject[] relatorioAvaliacao;
    public Text relatorioValor, relatorioTempo;
    Missao mostrandoMissao = null;

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
        UpdateMissaoDetails(null);
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

        UpdateSliders();
    }

    void UpdateBotaoInterromperMissao() {
        // Se jogador estiver em uma missão, mostra o botão de interromper
        interromperMissaoBtn.SetActive(MissaoManager.instance.missaoAtual != null);

        // Por algum motivo a unity não atualiza o layout automaticamente nesse caso, esta é a solução que encontrei
        Canvas.ForceUpdateCanvases();
        interromperMissaoBtn.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;
        interromperMissaoBtn.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
    }

    public void HandleBotaoInterromperMissao() {
        MissaoManager.instance.InterromperMissao();

        UpdateBotaoInterromperMissao();
    }

    GameObject GenerateMissaoItem(Missao missao) {
        ToggleGroup toggleGroup = missaoList.GetComponent<ToggleGroup>();

        GameObject missaoItem = Instantiate(missaoItemPrefab, missaoList.transform);
        missaoItem.GetComponent<RefMissao>().missao = missao;
        missaoItem.GetComponentInChildren<TextMeshProUGUI>().text = missao.titulo;

        Toggle toggle = missaoItem.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener(delegate { HandleMissaoItemClick(toggle); });

        return missaoItem;
    }

    public void GenerateMissaoList() {
        List<Missao> missoes = MissaoManager.instance.missoesDisponiveis;
        List<Missao> missoesConcluidas = MissaoManager.instance.missoesConcluidas;

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
        mostrandoMissao = missao;

        if (missao == null) {
            missaoDetails.SetActive(false);
            return;
        }

        missaoDetails.SetActive(true);

        // Text titulo = missaoDetails.transform.Find("Conteudo").Find("Titulo").GetComponent<Text>();
        Text descricao = missaoDetails.transform.Find("Conteudo").Find("Descricao").GetComponent<Text>();

        // missaoJaFoiConcluida.SetActive(missao.FoiFinalizada());
        // missaoAtual.SetActive(missao == MissaoManager.instance.missaoAtual);
        // titulo.text = missao.titulo;

        descricao.text = missao.descricao;

        string texto = "Os itens a serem entregues são:\n";
        Carga[] cargas = missao.GetAllCargas();
        foreach (Carga carga in cargas) {
            texto += " - " + carga.nome + "\n";
        }

        descricao.text += "\n\n" + texto;

        missaoSelecionadaRelatorio.SetActive(missao.melhorStatus != null);
        if (missao.melhorStatus != null) {
            relatorioValor.text = missao.melhorStatus.dinheiro.ToString();
            relatorioTempo.text = missao.melhorStatus.tempo.ToString("0.00") + "s";

            foreach (GameObject obj in relatorioAvaliacao) {
                obj.SetActive(false);
            }

            relatorioAvaliacao[missao.melhorStatus.avaliacao].SetActive(true);
        }
    }

    public void HandleTentarNovamenteMissao(){
        if (mostrandoMissao == null) return;

        mostrandoMissao.Resetar();
        UpdateBotaoInterromperMissao();
    }

    public void SetVolumeEfeito(float volume) {
        audioMixer.SetFloat("Efeitos", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeCarro(float volume) {
        audioMixer.SetFloat("Carro", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeGeral(float volume) {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);

        Debug.Log("Volume geral: " + volume);
        Debug.Log("Volume geral (log): " + Mathf.Log10(volume));
        Debug.Log("Volume geral (log) * 20: " + Mathf.Log10(volume) * 20);

        audioMixer.GetFloat("Master", out volume);
        Debug.Log("Volume geral (log) * 20 (int): " + volume);
    }

    public void UpdateSliders() {
        float volume;
        audioMixer.GetFloat("Efeitos", out volume);
        efeitoSlider.value = Mathf.Pow(10, volume / 20);

        audioMixer.GetFloat("Carro", out volume);
        carroSlider.value = Mathf.Pow(10, volume / 20);

        audioMixer.GetFloat("Master", out volume);
        geralSlider.value = Mathf.Pow(10, volume / 20);
    }

    public void Mostrar() {
        Tela tela = GetComponent<Tela>();
        tela.Mostrar();

        OpenMenu();
    }

    public void Esconder() {
        Tela tela = GetComponent<Tela>();
        tela.Esconder();
    }
}
