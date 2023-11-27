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

    NodoIA GetRandomNodo() {
        NodoIA nodoAtual = wnpc.GetTarget()?.GetComponent<NodoIA>();
        NodoIA novoNodo = wnpc.controller.GetRandomNodo();

        if (wnpc.controller.GetNodosVisitaveis().Count > 2) {
            while (novoNodo == nodoAtual) {
                novoNodo = wnpc.controller.GetRandomNodo();
            }
        }

        return novoNodo;
    }

    public void Enter() {
        NodoIA nodo = GetRandomNodo();
        nodosPath = SearchForNodoPathLargura(nodo);


        if (nodosPath == null) {
            wnpc.SetState(new PerambulaState(wnpc));
            return;
        }

        NodoIA nodoAtual = nodosPath.Dequeue();
        wnpc.SetTarget(nodoAtual.transform);
    }

    public void Execute(float dt) {
        Transform target = wnpc.GetTarget();

        if(target != null){
            wnpc.agent.SetDestination(target.position);

            if(wnpc.IsAtTarget()){
                NodoIA nodoAtual = target.GetComponent<NodoIA>();
                if (nodoAtual.descanso && wnpc.estaCansado) {
                    wnpc.SetState(new DescansoState(wnpc, this));
                } else if (nodosPath.Count > 0) {
                    nodoAtual = nodosPath.Dequeue();
                    wnpc.SetTarget(nodoAtual.transform);
                } else {
                    wnpc.SetState(new VisitandoState(wnpc));
                }
            }
        }
    }

    public void Exit() {
        nodosPath = null;
    }

    Queue<NodoIA> SearchForNodoPathProfundidade(NodoIA nodo) {
        if (nodo == null) return null;

        Stack<NodoComPai> nodos = new Stack<NodoComPai>();
        List<NodoIA> nodosVisitados = new List<NodoIA>();

        nodos.Push(new NodoComPai(nodo, null));

        while (nodos.Count > 0) {
            NodoComPai nodoAtual = nodos.Pop();
            nodosVisitados.Add(nodoAtual.nodo);

            if (nodoAtual.nodo == wnpc.GetTarget()?.GetComponent<NodoIA>()) {
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

    Queue<NodoIA> SearchForNodoPathLargura(NodoIA nodo) {
        if (nodo == null) return null;
        
        Queue<NodoComPai> nodos = new Queue<NodoComPai>();
        List<NodoIA> nodosVisitados = new List<NodoIA>();

        nodos.Enqueue(new NodoComPai(nodo, null));

        while (nodos.Count > 0) {
            NodoComPai nodoAtual = nodos.Dequeue();
            nodosVisitados.Add(nodoAtual.nodo);

            if (nodoAtual.nodo == wnpc.GetTarget()?.GetComponent<NodoIA>()) {
                Queue<NodoIA> nodosPath = new Queue<NodoIA>();

                while (nodoAtual != null) {
                    nodosPath.Enqueue(nodoAtual.nodo);
                    nodoAtual = nodoAtual.pai;
                }

                return nodosPath;
            }

            foreach (NodoIA nodoConectado in nodoAtual.nodo.nodosConectados) {
                if (!nodosVisitados.Contains(nodoConectado)) {
                    nodos.Enqueue(new NodoComPai(nodoConectado, nodoAtual));
                }
            }
        }

        return null;
    }
}
