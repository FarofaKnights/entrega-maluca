using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoComPai {
    public NodoIA nodo;
    public NodoComPai pai;

    public NodoComPai(NodoIA nodo, NodoComPai pai) {
        this.nodo = nodo;
        this.pai = pai;
    }
}

public class ObjetivoState : IState {
    WNPCMachine wnpc;
    Queue<NodoIA> nodosPath;
    

    public ObjetivoState(WNPCMachine wnpc){
        this.wnpc = wnpc;
    }

    public void Enter() {
        NodoIA nodo = NodoIA.GetRandomNodo();
        nodosPath = SearchForNodoPath(nodo);

        if (nodosPath == null) {
            wnpc.SetState(new PerambulaState(wnpc));
            Debug.Log("NodosPath is null");
            return;
        }

        Debug.Log("NodosPath count: " + nodosPath.Count);

        NodoIA nodoAtual = nodosPath.Dequeue();
        wnpc.target = nodoAtual.transform;
    }

    public void Update() {
        if(wnpc.target != null){
            wnpc.agent.SetDestination(wnpc.target.position);

            if(wnpc.IsAtTarget()){
                if (nodosPath.Count > 0) {
                    NodoIA nodoAtual = nodosPath.Dequeue();
                    wnpc.target = nodoAtual.transform;
                } else {
                    wnpc.SetState(new PerambulaState(wnpc));
                }
            }
        }
    }

    public void Exit() {
        nodosPath = null;
    }

    Queue<NodoIA> SearchForNodoPath(NodoIA nodo) {
        Stack<NodoComPai> nodos = new Stack<NodoComPai>();
        List<NodoIA> nodosVisitados = new List<NodoIA>();

        nodos.Push(new NodoComPai(nodo, null));

        while (nodos.Count > 0) {
            NodoComPai nodoAtual = nodos.Pop();
            nodosVisitados.Add(nodoAtual.nodo);

            Debug.Log("Nodo atual: " + nodoAtual.nodo.name);

            if (nodoAtual.nodo == wnpc.target.GetComponent<NodoIA>()) {
                Queue<NodoIA> nodosPath = new Queue<NodoIA>();

                while (nodoAtual != null) {
                    nodosPath.Enqueue(nodoAtual.nodo);
                    nodoAtual = nodoAtual.pai;
                }

                return nodosPath;
            }

            foreach (NodoIA nodoConectado in nodoAtual.nodo.nodosConectados) {
                if (!nodosVisitados.Contains(nodoConectado)) {
                    nodos.Push(new NodoComPai(nodoConectado, nodoAtual));
                }
            }
        }

        return null;
    }
}
