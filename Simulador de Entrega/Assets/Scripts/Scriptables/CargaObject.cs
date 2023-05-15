using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CargaObject {
    public float peso, fragilidade;
    public TipoCarga tipo;
    public string destinatario;

    public Carga Convert() {
        Endereco endereco = Endereco.GetEndereco(destinatario);
        return new Carga(peso, fragilidade, endereco, tipo);
    }
}
