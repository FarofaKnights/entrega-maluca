using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LimiteTempo: Limitacao {
    public float tempo;
    
    [System.NonSerialized]
    public Diretriz pai;

    bool falhou = false;

    Timer timer;

    public LimiteTempo(float tempo) {
        this.tempo = tempo;
    }

    public void Iniciar() {
        GameObject go = new GameObject("LimiteTempo");
        timer = go.AddComponent<Timer>();
        timer.tempo = tempo;
        timer.callback = Falhar;
        falhou = false;
    }

    public void Parar() {
        if (falhou)
            return;

        timer.Parar();
        timer = null;
    }

    public void Falhar() {
        falhou = true;
        Debug.Log("Limite de tempo atingido");
        pai.Falhar();
    }
}