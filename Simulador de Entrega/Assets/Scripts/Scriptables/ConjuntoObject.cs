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

        return new Conjunto(null, objetivos, sequencial, diretrizObj);
    }
}
