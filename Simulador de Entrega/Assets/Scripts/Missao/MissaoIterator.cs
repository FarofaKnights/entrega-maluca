using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissaoIterator: ObjetivoIterator {
    ConjuntoObject[] conjuntos;
    ConjuntoIterator iterator;
    int indice = 0;
    

    // Construtores
    public MissaoIterator(ConjuntoObject[] conjuntos) {
        this.conjuntos = conjuntos;
        iterator = new ConjuntoIterator(conjuntos[0]);
    }

    public Objetivo[] Next() {
        Objetivo[] objetivos = iterator.Next();

        if (objetivos == null) {
            indice++;

            if (indice >= conjuntos.Length) {
                return null;
            }

            iterator = new ConjuntoIterator(conjuntos[indice]);
            objetivos = iterator.Next();
        }
            
        return objetivos;
    }

    public Objetivo[] GetCurrent() {
        return iterator.GetCurrent();
    }

    public bool HasNext() {
        if (iterator.HasNext()) return true;
        return indice + 1 < conjuntos.Length;
    }

    public void Reset() {
        iterator.Reset();
        indice = 0;
        iterator = new ConjuntoIterator(conjuntos[0]);
    }
}
