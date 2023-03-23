using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoCarga { Normal, Rara, Importante, Especial };

[System.Serializable]
public class Carga {
    public float peso, fragilidade;
    public TipoCarga tipo;
    public Endereco destinatario;
    public Caixas cx;

    public Carga(float peso, float fragilidade, Endereco destinatario, TipoCarga tipo = TipoCarga.Normal) {
        this.peso = peso;
        this.fragilidade = fragilidade;
        this.destinatario = destinatario;
        this.tipo = tipo;
    }
}
