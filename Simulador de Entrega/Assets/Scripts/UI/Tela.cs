using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Tela : MonoBehaviour {
    public string nome;
    public bool exclusivo = false;

    Tela[] GetVizinhas() {
        List<Tela> vizinhas = new List<Tela>();
        
        foreach (Transform t in transform.parent) {
            Tela tela = t.GetComponent<Tela>();
            if (tela != null && tela != this) {
                vizinhas.Add(tela);
            }
        }

        return vizinhas.ToArray();
    }

    public void Mostrar() {
        if (exclusivo) {
            foreach (Tela t in GetVizinhas()) {
                t.Esconder();
            }
        }

        gameObject.SetActive(true);
    }

    public void Esconder() {
        gameObject.SetActive(false);
    }

    public Tela GetVizinho(string nome) {
        foreach (Tela t in GetVizinhas()) {
            if (t.nome == nome) {
                return t;
            }
        }

        return null;
    }

    public Tela GetParent() {
        return transform.parent?.GetComponent<Tela>();
    }
    

    public bool visivel {
        get { return gameObject.activeSelf; }
    }
}
