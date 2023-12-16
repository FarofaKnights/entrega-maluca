using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitandoState : IState {
    WNPCMachine wnpc;
    public float tempoVisitando;
    float timer;

    public VisitandoState(WNPCMachine wnpc) {
        this.wnpc = wnpc;
    }

    public void Enter() {
        tempoVisitando = Random.Range(3.0f, 6.0f);
        timer = 0;
        wnpc.animator.SetTrigger("Parar");
    }
    public void Execute(float deltaTime) {
        timer += deltaTime;

        if (timer >= tempoVisitando) {
            int chance = Random.Range(0, 2);
            if(chance == 0) wnpc.SetState(new PerambulaState(wnpc));
            else wnpc.SetState(new ObjetivoState(wnpc));
        }
    }

    public void Exit() { }
}
