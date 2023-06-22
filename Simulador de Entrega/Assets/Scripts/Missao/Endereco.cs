using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Endereco : MonoBehaviour {
    public static Dictionary<string, Endereco> ListaEnderecos = new Dictionary<string, Endereco>();

    public string nome;
    // public GameObject colisor;

    Objetivo objetivo;
    public IncluiMinimapa icone;

    void Awake() {
        ListaEnderecos.Add(nome, this);
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

    public void HandleTrigger(bool entrou) {
        if (objetivo != null) {
            objetivo.HandleObjetivoTrigger(entrou);
        }
    }

    public static Endereco GetEndereco(string nome) {
        return ListaEnderecos[nome];
    }

    void OnDestroy() {
        ListaEnderecos.Remove(nome);
    }
}
