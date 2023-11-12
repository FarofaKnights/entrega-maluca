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
        iterator.diretriz?.Iniciar();
    }

    public Objetivo[] Next() {
        Objetivo[] objetivos = iterator.Next();

        if (objetivos == null) {
            iterator.diretriz?.Parar();
            indice++;

            if (indice >= conjuntos.Length) {
                return null;
            }

            iterator = new ConjuntoIterator(conjuntos[indice]);
            iterator.diretriz?.Iniciar();
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
