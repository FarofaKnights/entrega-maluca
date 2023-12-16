using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WNPCMachine : MonoBehaviour {
    IState state;

    protected Transform target;
    public string estado;

    public WNPCController controller;
    public Animator animator;

    public bool estaCansado {
        get => energia >= energiaMax;
    }
    
    [HideInInspector]
    public NavMeshAgent agent;
    public Atropelavel atropelavel;

    public int energia = 0, energiaMax;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        atropelavel = GetComponent<Atropelavel>();
        atropelavel.OnAmassado += () => SetAmassado();

        SetState(new ObjetivoState(this));
        ResetEnergia();
    }

    void SetAmassado() {
        if (GetState().GetType() != typeof(AmassadoState))
            SetState(new AmassadoState(this, GetState()));
    }

    void FixedUpdate(){
        state?.Execute(Time.fixedDeltaTime);
    }

    public void SetState(IState state){
        this.state?.Exit();
        this.state = state;
        this.state?.Enter();

        estado = state.GetType().Name;
    }

    #region Metodos auxiliares

    public void SetTarget(Transform t) {
        if (t != target && t!= null)
            energia++;

        target = t;
    }

    public IState GetState() {
        return state;
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
