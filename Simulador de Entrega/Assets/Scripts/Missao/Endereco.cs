using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Endereco : MonoBehaviour {
    public static Dictionary<string, Endereco> ListaEnderecos = new Dictionary<string, Endereco>();

    public GameObject posicaoInicial;

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
        controls.Game.Disable();
    }

    public void EfetuarAcao() {
        objetivo.Concluir();
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
        }

        if (entrou) {
            controls.Game.EfetuarAcao.performed += ctx => EfetuarAcao();
        } else {
            controls.Game.EfetuarAcao.performed -= ctx => EfetuarAcao();
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