using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Endereco : MonoBehaviour {
    public static Dictionary<string, Endereco> ListaEnderecos = new Dictionary<string, Endereco>();
    public GameObject posicaoInicial;
    public GameObject indicadorBotao;
    public GameObject personagemHolder;
    GameObject personagemObj;
    bool playerIn = false;

    public string nome;
    public Sprite imagem;
    // public GameObject colisor;

    Objetivo objetivo;
    public IncluiMinimapa icone;
    Controls controls;

    public Sprite GetImagem() {
        if (imagem == null) return GameManager.instance.imagemSemEndereco;
        else return imagem;
    }

    void Awake() {
        ListaEnderecos.Add(nome, this);
        controls = new Controls();
    }

    void OnEnable() {
        if (controls != null)
            controls.Game.Enable();
        else {
            controls = new Controls();
            controls.Game.Enable();
        }

        if(!playerIn) indicadorBotao.SetActive(false);
    }

    void OnDisable() {
        playerIn = false;

        objetivo = null;
        indicadorBotao.SetActive(false);

        if (controls != null) {
            controls.Game.EfetuarAcao.performed -= EfetuarAcao;
            controls.Game.Disable();
            controls = null;
        }
    }

    public void EfetuarAcao() {
        if (!playerIn) return;

        objetivo.Concluir();
        UIController.HUD.MostrarBotaoAcao(null, false);
        UIController.HUD.MostrarMissaoInfo(null, false);
    }

    public void EfetuarAcao(InputAction.CallbackContext callback) {
        // Eu sei, parece não fazer sentido, por que eu não faço só um lambda?
        // Despois de quebrar a cabeça eu descobri que não tem como você desinscrever um lambda,
        // pois ele é considerado um outro método, então eu tive que fazer um método não anônimo
        EfetuarAcao();
    }

    public virtual void DefinirComoObjetivo(Objetivo objetivo) {
        this.objetivo = objetivo;
        gameObject.SetActive(true);

        if (icone != null) icone.AtivarIcone();

        if (objetivo is ObjetivoInicial) {
            PersonagemObject personagem = ((ObjetivoInicial) objetivo).missao.info.personagem;
            if (personagem != null) SetPersonagem(personagem);
        }
    }

    public GameObject SetPersonagem(PersonagemObject personagem) {
        if (personagemObj != null) Destroy(personagemObj);
        personagemObj = Instantiate(personagem.prefab, personagemHolder.transform);
        personagemObj.transform.localPosition = Vector3.zero;
        personagemObj.transform.localRotation = Quaternion.identity;

        Personagem personagemScript = personagemObj.GetComponent<Personagem>();
        personagemScript.Aguardar();

        return personagemObj;
    }

    public void RemovePersonagem() {
        if (personagemObj != null) Destroy(personagemObj);
    }

    public void PersonagemSecundaryTarget(Transform t) {
        personagemHolder.GetComponent<FacePlayer>().secundaryTarget = t;
    }

    public virtual void RemoverObjetivo() {
        Objetivo objetivo = this.objetivo;

        this.objetivo = null;
        gameObject.SetActive(false);

        if (objetivo is ObjetivoInicial) {
            if (((ObjetivoInicial) objetivo).missao.info.personagem != null)
                RemovePersonagem();
        }

        if (icone != null) icone.DesativarIcone();
    }

    public void HandleTrigger(bool entrou) {
        playerIn = entrou;

        if (objetivo != null) {
            objetivo.HandleObjetivoTrigger(entrou);
            if (objetivo is ObjetivoInicial) indicadorBotao.SetActive(false);
            else indicadorBotao.SetActive(entrou);
        }

        if (entrou) {
            if (controls == null) {
                controls = new Controls();
                controls.Game.Enable();
            }
            controls.Game.EfetuarAcao.performed += EfetuarAcao;
        } else {
            controls.Game.EfetuarAcao.performed -= EfetuarAcao;
        }
    }

    public void TeleportToHere() {
        Player.instance.transform.position = posicaoInicial.transform.position;
        Player.instance.transform.rotation = posicaoInicial.transform.rotation;
    }

    public static Endereco GetEndereco(string nome) {
        return ListaEnderecos[nome];
    }

    public static Endereco GetRandomEndereco() {
        Endereco endereco;

        do {
            int random = Random.Range(0, ListaEnderecos.Count);
            List<string> keyList = new List<string>(ListaEnderecos.Keys);
            string nome = keyList[random];
            endereco = ListaEnderecos[nome];
        } while (endereco.GetType() == typeof(EnderecoFalso));
        
        return endereco;
    }

    void OnDestroy() {
        ListaEnderecos.Remove(nome);
    }
}