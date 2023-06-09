using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUpgrade: MonoBehaviour {
    public string nome, descricao;

    public float custo;
    public bool comprado = false;
    public bool ativo = false;

    public int id = 0;

    public void Comprar() {
        if (comprado) {
            Ativar();
            return;
        }

        if (Player.instance.GetDinheiro() >= custo) {
            Player.instance.RemoverDinheiro(custo);
            comprado = true;

            OficinaController.instance.upgradesComprados.Add(this);

            Ativar();
        }
    }

    public abstract void Ativar();
    public abstract void Desativar();
}