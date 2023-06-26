using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjetivoObject {
    public string endereco;
    public List<CargaObject> cargas;
    public bool permiteReceber = false;
    public DiretrizObject diretriz;

    public virtual Objetivo Convert() {
        Endereco enderecoObj = Endereco.GetEndereco(endereco);
        Diretriz diretrizObj = diretriz != null ? diretriz.Convert() : null;
        if (diretrizObj != null && diretrizObj.texto == "") diretrizObj = null;

        List<Carga> cargasList = null;

        if (cargas != null) {
            cargasList  = new List<Carga>();
            foreach (CargaObject cargaObject in cargas) {
                cargasList.Add(cargaObject.Convert());
            }
        }

        Objetivo objetivo = new Objetivo(enderecoObj, cargasList, permiteReceber, diretrizObj);
        return objetivo;
    }
}