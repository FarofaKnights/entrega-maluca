using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Endereco : MonoBehaviour {
    public static Dictionary<string, Endereco> ListaEnderecos = new Dictionary<string, Endereco>();

    public string nome;
    // public GameObject colisor;

    Objetivo objetivo;
    IncluiMinimapa icone;

    void Awake() {
        ListaEnderecos.Add(nome, this);
        icone = GetComponent<IncluiMinimapa>();
    }

    public void DefinirComoObjetivo(Objetivo objetivo) {
        this.objetivo = objetivo;
        gameObject.SetActive(true);

        if (icone != null) icone.AtivarIcone();
    }

    public void RemoverObjetivo() {
        Objetivo objetivo = this.objetivo;

        this.objetivo = null;
        gameObject.SetActive(false);

        if (icone != null) icone.DesativarIcone();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && objetivo != null) {
            objetivo.HandleObjetivoTrigger(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player" && objetivo != null) {
            objetivo.HandleObjetivoTrigger(false);
        }
    }

    public static Endereco GetEndereco(string nome) {
        return ListaEnderecos[nome];
    }

    void OnDestroy() {
        ListaEnderecos.Remove(nome);
    }
}
