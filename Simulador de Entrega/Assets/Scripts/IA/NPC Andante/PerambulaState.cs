using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerambulaState : IState {
    WNPCMachine wnpc;

    public PerambulaState(WNPCMachine wnpc){
        this.wnpc = wnpc;
    }

    public void Enter() {}

    public void Update() {
        if(wnpc.target != null){
            wnpc.agent.SetDestination(wnpc.target.position);

            if(wnpc.IsAtTarget()){
                ChangeTarget();
            }
        }
    }

    public void Exit() {}


    void ChangeTarget() {
        NodoIA nodo = wnpc.target.GetComponent<NodoIA>();
        if(nodo != null){
            List<NodoIA> nodosConectados = nodo.nodosConectados;

            if (nodosConectados.Count == 1) {
                wnpc.target = nodosConectados[0].transform;
                return;
            }

            // Get random nodo that is not the current one
            NodoIA randomNodo = nodosConectados[Random.Range(0, nodosConectados.Count)];
            while (randomNodo == nodo) {
                randomNodo = nodosConectados[Random.Range(0, nodosConectados.Count)];
            }

            // Set new target
            wnpc.target = randomNodo.transform;
        }
    }
    
}
