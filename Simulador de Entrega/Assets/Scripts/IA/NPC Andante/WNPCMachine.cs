using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WNPCMachine : MonoBehaviour {
    IState state;

    protected Transform target;
    public string estado;

    public bool estaCansado {
        get => energia >= energiaMax;
    }
    
    [HideInInspector]
    public NavMeshAgent agent;

    public int energia = 0, energiaMax;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        SetState(new ObjetivoState(this));
        ResetEnergia();
    }

    void FixedUpdate(){
        state?.Update();
    }

    public void SetState(IState state){
        this.state?.Exit();
        this.state = state;
        this.state?.Enter();

        estado = state.GetType().Name;
    }

    #region Metodos auxiliares

    public void SetTarget(Transform t) {
        if (t != target)
            energia++;

        target = t;
    }

    public Transform GetTarget() {
        return target;
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

    public void ResetEnergia() {
        energia = 0;
        energiaMax = Random.Range(7, 15);
    }

    #endregion
}
