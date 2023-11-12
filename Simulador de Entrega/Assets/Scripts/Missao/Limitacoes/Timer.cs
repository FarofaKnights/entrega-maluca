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
        UIController.HUD.MostrarTimer(this);
    }

    void FixedUpdate() {
        tempoRestante -= Time.fixedDeltaTime;
        UIController.HUD.AtualizarTimer(tempoRestante);

        if (tempoRestante <= 0) {
            tempoRestante = 0;
            Destroy(gameObject);
        }
    }

    public void Parar() {
        callback = null;
        Destroy(gameObject);
    }

    void OnDestroy() {
        UIController.HUD.EsconderTimer(this);
        if (callback != null) {
            callback();
        }
    }
}
