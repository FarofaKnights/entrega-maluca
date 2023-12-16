using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Diretriz: Iniciavel {
    public string texto;
    public Limitacao[] limitacoes;
    int nivel = 2;

    public Diretriz(string texto, Limitacao[] limitacoes, int nivel = 2) {
        this.texto = texto;
        this.limitacoes = limitacoes;
        this.nivel = nivel;
    }

    public Diretriz(string texto, int nivel = 2) {
        this.texto = texto;
        this.nivel = nivel;
    }
    
    public void Iniciar() {
        if (this.texto != null && this.texto != "")
            UIController.diretriz.AddDiretriz(this, nivel);

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

    public void Parar() {
        if (this.texto != null && this.texto != "")
            UIController.diretriz.ConcluirDiretriz(this);

        if (limitacoes != null) {
            foreach (Limitacao limitacao in limitacoes) {
                limitacao.Parar();
            }
        }
    }
    
    public void Falhar() {
        Parar();
        MissaoManager.instance.FalharMissao();
    }

    public int GetNivel() {
        return nivel;
    }
}
