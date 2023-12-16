using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissaoIterator: ObjetivoIterator {
    ConjuntoObject[] conjuntos;
    ConjuntoIterator iterator;
    ConjuntoIterator iteratorFinal;
    int indice = 0;
    

    // Construtores
    public MissaoIterator(ConjuntoObject[] conjuntos) {
        this.conjuntos = conjuntos;
        iterator = CreateIterator(conjuntos[0]);
        iterator.diretriz?.Iniciar();
    }

    public MissaoIterator(ConjuntoObject[] conjuntos, CutsceneGroup cutsceneGroup) {
        this.conjuntos = conjuntos;
        indice = -1;

        iterator = CreateIterator(new Objetivo[] { cutsceneGroup.inicial }, null);
        iterator.diretriz?.Iniciar();

        if (cutsceneGroup.final != null)
            iteratorFinal = CreateIterator(new Objetivo[] { cutsceneGroup.final }, null);
    }

    public Objetivo[] Next() {
        Objetivo[] objetivos = iterator.Next();

        if (objetivos == null) {
            iterator.diretriz?.Parar();
            indice++;

            if (indice == conjuntos.Length && iteratorFinal != null) {
                iterator = iteratorFinal;
                iterator.diretriz?.Iniciar();
                objetivos = iterator.Next();
                return objetivos;
            } else if (indice > conjuntos.Length) {
                return null;
            }

            iterator = CreateIterator(conjuntos[indice]);
            iterator.diretriz?.Iniciar();
            objetivos = iterator.Next();
        }
            
        return objetivos;
    }

    ConjuntoIterator CreateIterator(ConjuntoObject conjunto) {
        if (!conjunto.sequencial) return new NaoLinearConjuntoIterator(conjunto);
        return new ConjuntoIterator(conjunto);
    }

    ConjuntoIterator CreateIterator(Objetivo[] objetivos, Diretriz diretriz) {
        return new ConjuntoIterator(objetivos, diretriz);
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
        iterator = null;
    }
}
