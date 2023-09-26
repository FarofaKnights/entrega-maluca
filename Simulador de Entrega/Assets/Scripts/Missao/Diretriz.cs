using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Diretriz: Iniciavel {
    public string texto;
    public Iniciavel pai;
    public Limitacao[] limitacoes;

    public Diretriz(string texto, Limitacao[] limitacoes) {
        this.texto = texto;
        this.limitacoes = limitacoes;
        this.pai = null;
    }

    public Diretriz(string texto) {
        this.texto = texto;
        this.pai = null;
    }
    
    public void Iniciar() {
        if (this.texto != null && this.texto != "")
            UIController.HUD.AdicionarDiretriz(this);

        if (limitacoes != null) {
            foreach (Limitacao limitacao in limitacoes) {
                // Definitivamente vou ter que mudar isso depois
                if (limitacao is LimiteTempo) {
                    LimiteTempo limiteTempo = (LimiteTempo) limitacao;
                    limiteTempo.pai = this;
                }
                limitacao.Iniciar();
            }
        }
    }

    public void Interromper() {
        if (this.texto != null && this.texto != "")
            UIController.HUD.RemoverDiretriz(this);

        if (limitacoes != null) {
            foreach (Limitacao limitacao in limitacoes) {
                limitacao.Interromper();
            }
        }
    }

    public void Falhar() {
        Interromper();
        pai.Interromper();
    }
}
