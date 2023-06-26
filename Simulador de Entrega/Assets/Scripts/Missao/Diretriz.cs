using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Diretriz: Iniciavel {
    public string texto;
    public Iniciavel pai;
    public Limitacao[] limitacoes;

    public Diretriz(string texto) {
        this.texto = texto;
        this.pai = null;
    }
    
    public void Iniciar() {
        if (this.texto != null && this.texto != "")
            UIController.instance.AdicionarDiretriz(this);
    }

    public void Interromper() {
        if (this.texto != null && this.texto != "")
            UIController.instance.RemoverDiretriz(this);
    }

    public void Falhar() {
        Interromper();
        pai.Interromper();
    }
}
