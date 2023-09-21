using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer: MonoBehaviour {
    public float tempo;
    float tempoRestante;

    public Action callback;

    void Start() {
        tempoRestante = tempo;
        UIController.instance.MostrarTimer(true);
    }

    void FixedUpdate() {
        tempoRestante -= Time.fixedDeltaTime;
        UIController.instance.AtualizarTimer(tempoRestante);

        if (tempoRestante <= 0) {
            tempoRestante = 0;
            Destroy(gameObject);
        }
    }

    public void Interromper() {
        callback = null;
        Destroy(gameObject);
    }

    void OnDestroy() {
        UIController.instance.MostrarTimer(false);
        if (callback != null) {
            callback();
        }
    }
}
