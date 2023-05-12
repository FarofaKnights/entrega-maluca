using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Diretriz: Iniciavel {
    public string descricao;
    public Iniciavel pai;

    public Diretriz(string descricao) {
        this.descricao = descricao;
        this.pai = null;
    }
    
    public void Iniciar() {
        Debug.Log("Diretriz: " + descricao);
    }

    public void Interromper() {
        Debug.Log("Diretriz interrompida: " + descricao);
    }

    public void Finalizar() {
        Debug.Log("Diretriz finalizada: " + descricao);
    }
}
