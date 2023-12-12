using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeObject : ScriptableObject {
    public string nome; // Unique KEY
    public string descricao;
    public float custo;

    public bool exclusivo;
    public bool ativavel = true;

    public bool comprado {
        get {
            return OficinaController.instance.IsUpgradeComprado(this);
        }
    }

    public bool ativo {
        get {
            return OficinaController.instance.IsUpgradeAtivo(this);
        }
    }

    public void Ativar() {
        _Ativar();

        if (!OficinaController.instance.IsUpgradeAtivo(this)) {
            OficinaController.instance.AtivarUpgrade(this);
        }
    }

    public void Desativar() {
        _Desativar();

        if (OficinaController.instance.IsUpgradeAtivo(this)) {
            OficinaController.instance.DesativarUpgrade(this);
        }
    }

    public void Comprar() {
        OficinaController.instance.ComprarUpgrade(this);
    }

    protected abstract void _Ativar();
    protected abstract void _Desativar();
    public abstract bool CalculoDeExclusividade(UpgradeObject outro);
    public abstract Dictionary<string, (string, float)> GetInfo();
}
