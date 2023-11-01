using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerambulaState : IState {
    WNPCMachine wnpc;

    public PerambulaState(WNPCMachine wnpc){
        this.wnpc = wnpc;
    }

    public void Enter() {}

    public void Execute(float deltaTime) {
        if(wnpc.GetTarget() != null){
            wnpc.agent.SetDestination(wnpc.GetTarget().position);

            if(wnpc.IsAtTarget()){
                ChangeTarget();
            }
        }
    }

    public void Exit() {}


    void ChangeTarget() {
        NodoIA nodo = wnpc.GetTarget().GetComponent<NodoIA>();
        if(nodo != null){
            if (nodo.descanso && wnpc.estaCansado) {
                wnpc.SetState(new DescansoState(wnpc, this));
                return;
            }

            if (nodo.visitavel) { 
                int chance = Random.Range(0, 10);
                if (chance == 0) wnpc.SetState(new VisitandoState(wnpc));
                return;
            }

            List<NodoIA> nodosConectados = nodo.nodosConectados;

            if (nodosConectados.Count == 1) {
                wnpc.SetTarget(nodosConectados[0].transform);
                return;
            }

            // Get random nodo that is not the current one
            NodoIA randomNodo = nodosConectados[Random.Range(0, nodosConectados.Count)];
            while (randomNodo == nodo) {
                randomNodo = nodosConectados[Random.Range(0, nodosConectados.Count)];
            }

            // Set new target
            wnpc.SetTarget(randomNodo.transform);
        }
    }
    
}
