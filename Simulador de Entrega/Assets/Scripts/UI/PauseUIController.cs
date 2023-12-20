using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PauseUIController : MonoBehaviour {
    public GameObject missaoPanel, menuPanel, opcoesPanel;
    public GameObject missaoDetails, missaoList, missaoItemPrefab;

    public GameObject missaoJaFoiConcluida, missaoAtual;

    public AudioMixer audioMixer;
    public Slider efeitoSlider, carroSlider, geralSlider;

    public GameObject missaoSelecionadaRelatorio;
    public GameObject[] relatorioAvaliacao;
    public Text relatorioValor, relatorioTempo;
    Missao mostrandoMissao = null;

    public TextMeshProUGUI descricaoGrandeMissao;
    public Image enderecoMissao;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
    }

    public void OpenMissao() {
        UIController.diretriz.Hide();
        missaoPanel.SetActive(true);
        opcoesPanel.SetActive(false);
        menuPanel.SetActive(false);
        GenerateMissaoList();
        UpdateMissaoDetails(null);
    }

    public void OpenMenu() {
        UIController.diretriz.Show();
        missaoPanel.SetActive(false);
        opcoesPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OpenOpcoes() {
        UIController.diretriz.Hide();
        missaoPanel.SetActive(false);
        opcoesPanel.SetActive(true);
        menuPanel.SetActive(false);

        UpdateSliders();
    }


    public void HandleBotaoInterromperMissao() {
        MissaoManager.instance.PararMissao();
    }

    public void HandleBotaoReiniciarMissao() {
        GameManager.instance.Despausar();
        MissaoManager.instance.ReiniciarMissao(MissaoManager.instance.missaoAtual);
    }

    GameObject GenerateMissaoItem(Missao missao) {
        ToggleGroup toggleGroup = missaoList.GetComponent<ToggleGroup>();

        GameObject missaoItem = Instantiate(missaoItemPrefab, missaoList.transform);
        missaoItem.GetComponent<RefMissao>().missao = missao;
        missaoItem.GetComponentInChildren<TextMeshProUGUI>().text = missao.info.nome;

        Toggle toggle = missaoItem.GetComponent<Toggle>();
        toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener(delegate { HandleMissaoItemClick(toggle); });

        return missaoItem;
    }

    public void GenerateMissaoList() {
        Missao[] missoes = MissaoManager.instance.GetMissoesDisponiveis();
        Missao[] missoesConcluidas = MissaoManager.instance.GetMissoesConcluidas();

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

        descricao.text = missao.info.descricao;
        descricaoGrandeMissao.text = (missao.info.descricaoGrande != null && missao.info.descricaoGrande!="") ? missao.info.descricaoGrande : "Estou precisando de um serviço de entrega, entre em contato para mais informações.";
        enderecoMissao.sprite = missao.info.GetEnderecoFinal().GetImagem();


        Carga[] cargas = missao.info.GetAllCargas();

        if (cargas != null && cargas.Length > 0) {
            string texto = "Os itens a serem entregues são:\n";        
            foreach (Carga carga in cargas) {
                string nome = carga.nome != null ? carga.nome : carga.prefab.GetComponent<Caixa>().carga.nome;
                texto += " - " + nome + "\n";
            }

            descricao.text += "\n\n" + texto;
        }

        missaoSelecionadaRelatorio.SetActive(missao.melhorStatus != null);
        if (missao.melhorStatus != null) {
            relatorioValor.text = missao.melhorStatus.dinheiro.ToString();
            relatorioTempo.text = missao.melhorStatus.tempo.ToString("0.00") + "s";

            foreach (GameObject obj in relatorioAvaliacao) {
                obj.SetActive(false);
            }

            relatorioAvaliacao[missao.melhorStatus.avaliacao-1].SetActive(true);
        }
    }

    public void HandleTentarNovamenteMissao(){
        if (mostrandoMissao == null) return;
        GameManager.instance.Despausar();
        MissaoManager.instance.ReiniciarMissao(mostrandoMissao);
    }

    public void SetVolumeEfeito() {
        float volume = efeitoSlider.value;
        audioMixer.SetFloat("Efeitos", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeCarro() {
        float volume = carroSlider.value;
        audioMixer.SetFloat("Carro", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeGeral() {
        float volume = geralSlider.value;
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
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

    public void SairPause() {
        GameManager.instance.Despausar();
    }

    public void SairJogo() {
        GameManager.instance.VoltarMenu();
    }

    public void Mostrar() {
        Tela tela = GetComponent<Tela>();
        tela.Mostrar();

        OpenMenu();
    }

    public void Esconder() {
        Tela tela = GetComponent<Tela>();
        tela.Esconder();
        UIController.diretriz.Hide();
    }
}
