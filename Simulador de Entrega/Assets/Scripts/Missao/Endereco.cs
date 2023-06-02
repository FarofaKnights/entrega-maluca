using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Endereco : MonoBehaviour {
    public static Dictionary<string, Endereco> ListaEnderecos = new Dictionary<string, Endereco>();

    public string nome;
    // public GameObject colisor;

    Objetivo objetivo;

    void Awake() {
        ListaEnderecos.Add(nome, this);
    }

    public void DefinirComoObjetivo(Objetivo objetivo) {
        this.objetivo = objetivo;
        gameObject.SetActive(true);
    }

    public void RemoverObjetivo() {
        this.objetivo = null;
        gameObject.SetActive(false);
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
}
