using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrosNPC : MonoBehaviour
{
    public Transform [] wayPoints;
    public Transform[] stopPoints;
    public Transform currentWaypoint;
    ComportamentoCarros comportamento; 
    public int currentWaypointNumber;
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        comportamento = new Mover(this, agent);
        comportamento.Enter();
    }

    void FixedUpdate()
    {
        comportamento.Action();
    }

   public void MudarComportamento(ComportamentoCarros c)
    {
        comportamento = c;
        comportamento.Enter();
    }
}
