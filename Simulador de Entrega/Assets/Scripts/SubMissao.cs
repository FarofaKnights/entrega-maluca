using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubMissao {
    [System.NonSerialized]
    public Missao missao;

    public Destino[] destinos;
    public bool sequencial = true;
    int indice = 0; // No sequencial, a posição do destino atual, no nao sequencial, a quantidade de destinos concluidos

    public SubMissao(Missao missao, Destino[] destinos, bool sequencial = true) {
        this.missao = missao;
        this.destinos = destinos;
        this.sequencial = sequencial;

        foreach (Destino destino in destinos) destino.pai = this;
    }

    public void Iniciar() {
        if (!sequencial) {
            foreach (Destino destino in destinos) destino.Iniciar();
        } else {
            destinos[0].Iniciar();
        }
    }

    public void Interromper() {
        if (sequencial) destinos[indice].Interromper();
        else foreach (Destino destino in destinos) destino.Interromper();

        indice = 0;
    }

    public void Finalizar() {
        missao.ProximaSubMissao();
    }

    // Chamado quando um destino foi concluido, recebendo como parametro o proprio destino
    public void DestinoConcluido(Destino destino) {
        if (sequencial) {
            Debug.Log("Sequencial");
            if (indice >= destinos.Length - 1) {
                Debug.Log("Finalizar");
                Finalizar();
                return;
            }

            indice++;
            destinos[indice].Iniciar();
        } else {
            Debug.Log("Não Sequencial");
            indice++;
            if (indice >= destinos.Length) {
                Finalizar();
            }
        }
    }
}
