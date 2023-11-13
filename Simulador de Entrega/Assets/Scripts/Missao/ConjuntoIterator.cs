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
        this.diretriz = conjunto.diretriz.Convert(1);

        for (int i = 0; i < objetivos.Length; i++) {
            objetivos[i] = conjunto.objetivos[i].Convert();
        }
    }
    

    public virtual Objetivo[] Next() {
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

    public virtual Objetivo[] GetCurrent() {
        if (indice < 0) return null;
        return new Objetivo[] {objetivos[indice]};
    }

    public virtual void Reset() {
        if (indice >= 0) objetivos[indice].Parar();
        indice = -1;
    }

    public virtual bool HasNext() {
        return indice + 1 < objetivos.Length;
    }
}

public class NaoLinearConjuntoIterator: ConjuntoIterator {
    public NaoLinearConjuntoIterator(Objetivo[] objetivos, Diretriz diretriz = null): base(objetivos, diretriz) {}

    public NaoLinearConjuntoIterator(ConjuntoObject conjunto): base(conjunto) {}

    public override Objetivo[] Next() {
        if (!HasNext()) return null;

        List<Objetivo> objetivos = new List<Objetivo>();
        foreach (Objetivo objetivo in this.objetivos) {
            if (!objetivo.IsConcluido()) {
                objetivos.Add(objetivo);

                if (!objetivo.IsIniciada()) {
                    objetivo.Iniciar();
                }
            }
        }

        return objetivos.ToArray();
    }

    public override Objetivo[] GetCurrent() {
        List<Objetivo> objetivos = new List<Objetivo>();
        foreach (Objetivo objetivo in this.objetivos) {
            if (!objetivo.IsConcluido()) {
                objetivos.Add(objetivo);
            }
        }

        return objetivos.ToArray();
    }

    public override void Reset() {
        foreach (Objetivo objetivo in objetivos) {
            objetivo.Parar();
        }
    }

    public override bool HasNext() {
        foreach (Objetivo objetivo in objetivos) {
            if (!objetivo.IsConcluido()) {
                return true;
            }
        }

        return false;
    }
}
