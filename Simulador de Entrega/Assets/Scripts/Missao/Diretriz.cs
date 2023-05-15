using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Diretriz: Iniciavel {
    public string texto;
    public Iniciavel pai;

    public Diretriz(string texto) {
        this.texto = texto;
        this.pai = null;
    }
    
    public void Iniciar() {
        // Debug.Log("Diretriz: " + texto);

        UIController.instance.AdicionarDiretriz(this);
    }

    public void Interromper() {
        // Debug.Log("Diretriz interrompida: " + texto);

        UIController.instance.RemoverDiretriz(this);
    }

    public void Finalizar() {
        // Debug.Log("Diretriz finalizada: " + texto);

        UIController.instance.RemoverDiretriz(this);
    }
}
