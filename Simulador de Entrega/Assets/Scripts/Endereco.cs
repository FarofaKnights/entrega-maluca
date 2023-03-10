using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endereco : MonoBehaviour {
    public static Dictionary<string, Endereco> ListaEnderecos = new Dictionary<string, Endereco>();

    public string nome;
    // public GameObject colisor;

    Destino destino;

    void Awake() {
        ListaEnderecos.Add(nome, this);
    }

    public void DefinirComoDestino(Destino destino) {
        this.destino = destino;
        gameObject.SetActive(true);
    }

    public void RemoverDestino() {
        this.destino = null;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && destino != null) {
            destino.HandleDestinoTrigger(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player" && destino != null) {
            destino.HandleDestinoTrigger(false);
        }
    }
}
