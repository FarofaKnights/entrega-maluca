using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conjunto: Iniciavel {
    [System.NonSerialized]
    public Missao missao;

    public Objetivo[] objetivos;
    public bool sequencial = true;

    public Diretriz diretriz;
    
    [SerializeField]
    int indice = 0; // No sequencial, a posição do objetivo atual, no nao sequencial, a quantidade de objetivos concluidos

    bool ativo = false;

    public Conjunto(Missao missao, Objetivo[] objetivos, bool sequencial = true, Diretriz diretriz = null) {
        this.missao = missao;
        this.objetivos = objetivos;
        this.sequencial = sequencial;
        this.diretriz = diretriz;

        foreach (Objetivo objetivo in objetivos) objetivo.pai = this;
    }

    // Metodos Iniciavel
    public void Iniciar() {
        if (ativo) return;
        ativo = true;

        if (!sequencial) {
            foreach (Objetivo objetivo in objetivos) objetivo.Iniciar();
        } else {
            objetivos[0].Iniciar();
        }

        if (diretriz != null) diretriz.Iniciar();
    }

    public void Interromper() {
        if (!ativo) return;
        ativo = false;

        if (sequencial) objetivos[indice].Interromper();
        else foreach (Objetivo objetivo in objetivos) objetivo.Interromper();

        indice = 0;

        if (diretriz != null) diretriz.Interromper();
    }

    void Finalizar() {
        if (!ativo) return;
        ativo = false;
        
        indice = 0;
        missao.ProximoConjunto();

        if (diretriz != null) diretriz.Interromper();
    }

    // Chamado quando um objetivo foi concluido, recebendo como parametro o proprio objetivo
    public void ObjetivoConcluido(Objetivo objetivo) {
        if (sequencial) {
            if (indice >= objetivos.Length - 1) {
                Finalizar();
                return;
            }

            indice++;
            objetivos[indice].Iniciar();
        } else {
            indice++;
            if (indice >= objetivos.Length) {
                Finalizar();
            }
        }
    }
}
