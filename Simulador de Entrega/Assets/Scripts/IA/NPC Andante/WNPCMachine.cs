using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WNPCMachine : MonoBehaviour {
    IState state;

    public Transform target;

    public NavMeshAgent agent;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        SetState(new ObjetivoState(this));
    }

    void FixedUpdate(){
        state?.Update();
    }

    public void SetState(IState state){
        this.state?.Exit();
        this.state = state;
        this.state?.Enter();
    }

    #region Metodos auxiliares
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

    #endregion
}
