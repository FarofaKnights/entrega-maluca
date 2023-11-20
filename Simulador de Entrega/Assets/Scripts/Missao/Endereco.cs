using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Endereco : MonoBehaviour {
    public static Dictionary<string, Endereco> ListaEnderecos = new Dictionary<string, Endereco>();

    public GameObject posicaoInicial;
    public GameObject indicadorBotao;

    public string nome;
    // public GameObject colisor;

    Objetivo objetivo;
    public IncluiMinimapa icone;
    Controls controls;

    void Awake() {
        ListaEnderecos.Add(nome, this);
        controls = new Controls();
    }

    void OnEnable() {
        controls.Game.Enable();
    }

    void OnDisable() {
        objetivo = null;
        controls.Game.EfetuarAcao.performed -= EfetuarAcao;
        controls.Game.Disable();
    }

    public void EfetuarAcao() {
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
    }

    public virtual void RemoverObjetivo() {
        Objetivo objetivo = this.objetivo;

        this.objetivo = null;
        gameObject.SetActive(false);

        if (icone != null) icone.DesativarIcone();
    }

    public void HandleTrigger(bool entrou) {
        if (objetivo != null) {
            objetivo.HandleObjetivoTrigger(entrou);
            if (objetivo is ObjetivoInicial) indicadorBotao.SetActive(false);
            else indicadorBotao.SetActive(entrou);
        }

        if (entrou) {
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