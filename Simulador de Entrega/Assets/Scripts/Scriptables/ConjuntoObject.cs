using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConjuntoObject {
    public ObjetivoObject[] objetivos;
    public bool sequencial = true;
    public DiretrizObject diretriz;

    public Conjunto Convert() {
        Objetivo[] objetivos = new Objetivo[this.objetivos.Length];
        Diretriz diretrizObj = diretriz != null ? diretriz.Convert() : null;
        if (diretrizObj != null && diretrizObj.texto == "") diretrizObj = null;

        for (int i = 0; i < objetivos.Length; i++) {
            objetivos[i] = this.objetivos[i].Convert();
        }

        Conjunto conj = new Conjunto(null, objetivos, sequencial, diretrizObj);

        if (diretrizObj != null) {
            diretrizObj.pai = conj;
        }

        if (objetivos != null) {
            foreach (Objetivo obj in objetivos) {
                obj.pai = conj;
            }
        }

        return conj;
    }
}
