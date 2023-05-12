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

    public Conjunto(Missao missao, Objetivo[] objetivos, bool sequencial = true) {
        this.missao = missao;
        this.objetivos = objetivos;
        this.sequencial = sequencial;
        this.diretriz = null;

        foreach (Objetivo objetivo in objetivos) objetivo.pai = this;
    }

    // Metodos Iniciavel
    public void Iniciar() {
        if (!sequencial) {
            foreach (Objetivo objetivo in objetivos) objetivo.Iniciar();
        } else {
            objetivos[0].Iniciar();
        }

        if (diretriz != null) diretriz.Iniciar();
    }

    public void Interromper() {
        if (sequencial) objetivos[indice].Interromper();
        else foreach (Objetivo objetivo in objetivos) objetivo.Interromper();

        indice = 0;

        if (diretriz != null) diretriz.Iniciar();
    }

    public void Finalizar() {
        indice = 0;
        missao.ProximoConjunto();
    }

    // Chamado quando um objetivo foi concluido, recebendo como parametro o proprio objetivo
    public void ObjetivoConcluido(Objetivo objetivo) {
        if (sequencial) {
            Debug.Log("Sequencial");
            if (indice >= objetivos.Length - 1) {
                Debug.Log("Finalizar");
                Finalizar();
                return;
            }

            indice++;
            objetivos[indice].Iniciar();
        } else {
            Debug.Log("Não Sequencial");
            indice++;
            if (indice >= objetivos.Length) {
                Finalizar();
            }
        }
    }
}
