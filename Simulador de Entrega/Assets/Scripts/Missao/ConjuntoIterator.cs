using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConjuntoIterator: ObjetivoIterator {
    protected Objetivo[] objetivos;
    public Diretriz diretriz;
    int indice = -1;


    public ConjuntoIterator(Objetivo[] objetivos, Diretriz diretriz = null) {
        this.objetivos = objetivos;
        this.diretriz = diretriz;
    }

    public ConjuntoIterator(ConjuntoObject conjunto) {
        this.objetivos = new Objetivo[conjunto.objetivos.Length];
        this.diretriz = conjunto.diretriz.Convert();

        for (int i = 0; i < objetivos.Length; i++) {
            objetivos[i] = conjunto.objetivos[i].Convert();
        }
    }
    

    public Objetivo[] Next() {
        // TODO: Checar se todos os objetivos estão marcados como concluídos

        if (indice >= 0) {
            objetivos[indice].Parar();
        }
        
        if (!HasNext()) return null;

        indice++;
        Objetivo objetivo = objetivos[indice];
        objetivo.Iniciar();

        return new Objetivo[] {objetivo};
    }

    public Objetivo[] GetCurrent() {
        if (indice < 0) return null;
        return new Objetivo[] {objetivos[indice]};
    }

    public void Reset() {
        if (indice >= 0) objetivos[indice].Parar();
        indice = -1;
    }

    public bool HasNext() {
        return indice + 1 < objetivos.Length;
    }
}
