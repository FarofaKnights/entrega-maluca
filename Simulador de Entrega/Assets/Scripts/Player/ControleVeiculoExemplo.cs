using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleVeiculoExemplo : MonoBehaviour {
    public float velocidade = 1;
    float _velocidade = 1;
    float velocidadeBase;
    Transform[] rodas;

    void Start() {
        rodas = new Transform[4];

        Transform paiRodas = transform.Find("Carro/Rodas");

        for (int i = 0; i < 4; i++) {
            rodas[i] = paiRodas.GetChild(i);
        }

        velocidadeBase = rodas[0].GetComponent<Rodas>().torqueNaRoda;
    }

    void UpdateSpeed() {
        if (velocidade == _velocidade) return;

        foreach (Transform roda in rodas) {
            roda.GetComponent<Rodas>().torqueNaRoda = velocidadeBase * velocidade;
        }

        _velocidade = velocidade;
    }

    void Update() {
        UpdateSpeed();
    }
}
