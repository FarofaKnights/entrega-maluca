using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoCarga { Normal, Rara, Importante, Especial };

[System.Serializable]
public class Carga {
    public float peso, fragilidade;
    float _fragilidadeInicial;
    public TipoCarga tipo;
    public Endereco destinatario;
    public Caixa cx;
    public GameObject prefab;

    public string nome {
        get {
            return prefab.name;
        }
    }

    public float fragilidadeInicial {
        get {
            return _fragilidadeInicial;
        }

        set {
            _fragilidadeInicial = value;
        }
    }

    public Carga(float peso, float fragilidade, Endereco destinatario, GameObject prefab, TipoCarga tipo = TipoCarga.Normal) {
        this.peso = peso;
        this.fragilidade = fragilidade;
        this.destinatario = destinatario;
        this.prefab = prefab;
        this.tipo = tipo;

        _fragilidadeInicial = fragilidade;
    }

    public Carga(float peso, float fragilidade, Endereco destinatario, TipoCarga tipo = TipoCarga.Normal)
    {
        this.peso = peso;
        this.fragilidade = fragilidade;
        this.destinatario = destinatario;
        this.prefab = GameManager.instance.prefabGenerico;
        this.tipo = tipo;

        _fragilidadeInicial = fragilidade;
    }

    public virtual float GetValor() {
        // Calculo e valores temporarios
        float valor = 0;

        switch (tipo) {
            case TipoCarga.Normal:
                valor = 10;
                break;
            case TipoCarga.Rara:
                valor = 20;
                break;
            case TipoCarga.Importante:
                valor = 30;
                break;
            case TipoCarga.Especial:
                valor = 40;
                break;
        }

        return valor;
    }

    public virtual float GetMaxValor() {
        return GetValor();
    }
}

// Exemplo de carga especial
public class CargaEspecial: Carga {
    public CargaEspecial(float peso, float fragilidade, Endereco destinatario, GameObject prefab) : base(peso, fragilidade, destinatario, prefab, TipoCarga.Especial) { }

    public override float GetValor() {
        return 40 + 10 * fragilidade;
    }

    public override float GetMaxValor() {
        return 40 + 10; // * fragilidade; //?????????
    }
}