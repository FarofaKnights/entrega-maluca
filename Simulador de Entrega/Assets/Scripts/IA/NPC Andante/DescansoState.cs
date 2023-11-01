using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescansoState : IState {
    WNPCMachine wnpc;
    IState estadoAnterior;

    public float tempoVisitando;
    float timer;

    public DescansoState(WNPCMachine wnpc, IState estadoAnterior) {
        this.wnpc = wnpc;
        this.estadoAnterior = estadoAnterior;
    }

    public void Enter() {
        tempoVisitando = Random.Range(2.0f, 4.0f);
        timer = 0;
    }
    public void Execute(float deltaTime) {
        timer += deltaTime;

        if (timer >= tempoVisitando)  {
            wnpc.ResetEnergia();
            wnpc.SetState(estadoAnterior);
        }
    }

    public void Exit() { }
}
