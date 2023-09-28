using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WNPC : MonoBehaviour {
    public Transform target;
    NavMeshAgent agent;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate(){
        if(target != null){
            agent.SetDestination(target.position);

            if(IsAtTarget()){
                ChangeTarget();
            }
        }
    }

    public bool IsAtTarget(){
        if (!agent.pathPending) {
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    return true;
                }
            }
        }

        return false;
    }
    
    void ChangeTarget() {
        NodoIA nodo = target.GetComponent<NodoIA>();
        if(nodo != null){
            List<NodoIA> nodosConectados = nodo.nodosConectados;

            if (nodosConectados.Count == 1) {
                target = nodosConectados[0].transform;
                return;
            }

            // Get random nodo that is not the current one
            int randomIndex = Random.Range(0, nodosConectados.Count);
            NodoIA randomNodo = nodosConectados[randomIndex];

            while (randomNodo == nodo) {
                randomIndex = Random.Range(0, nodosConectados.Count);
                randomNodo = nodosConectados[randomIndex];
            }

            // Set new target
            target = randomNodo.transform;
        }
    }
}
