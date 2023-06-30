using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {
    public static Player instance;

    public float dinheiro = 300;

    void Awake() {
        instance = this;
    }

    #region SistemaDeDinheiro

    public void AdicionarDinheiro(float valor) {
        dinheiro += valor;

        UIController.instance.AtualizarDinheiro();
    }

    public bool RemoverDinheiro(float valor) {
        if (dinheiro >= valor) {
            dinheiro -= valor;
            UIController.instance.AtualizarDinheiro();

            return true;
        }

        return false;
    }

    public float GetDinheiro() {
        return dinheiro;
    }

    #endregion
}
