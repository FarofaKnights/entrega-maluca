using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmassadoState : IState {
    WNPCMachine wnpc;
    IState estadoAnterior;

    public float tempoRecuperar;
    float timer;

    bool recuperou = false;

    public AmassadoState(WNPCMachine wnpc, IState estadoAnterior) {
        this.wnpc = wnpc;
        this.estadoAnterior = estadoAnterior;
    }

    public void Enter() {
        tempoRecuperar = Random.Range(2.0f, 4.0f);
        timer = 0;
        wnpc.animator.SetTrigger("Parar");
        wnpc.atropelavel.OnDesamassado += Desamassou;
        wnpc.agent.isStopped = true;
    }
    public void Execute(float deltaTime) {
        if (recuperou) return;

        timer += deltaTime;

        if (timer >= tempoRecuperar)  {
            wnpc.ResetEnergia();
            recuperou = true;
        }
    }

    void Desamassou() {
        wnpc.atropelavel.OnDesamassado -= Desamassou;
        wnpc.SetState(estadoAnterior);
    }

    public void Exit() {
        wnpc.agent.isStopped = false;
    }
}
