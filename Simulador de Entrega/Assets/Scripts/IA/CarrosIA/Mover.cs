using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : ComportamentoCarros
{
    CarrosNPC npc;
    NavMeshAgent ia;
    bool podeMudar = true;
    public Mover(CarrosNPC carro, NavMeshAgent agent)
    {
        npc = carro;
        ia = agent;
    }
    public void Enter()
    {
        npc.currentWaypointNumber++;
        if (npc.currentWaypointNumber == npc.wayPoints.Length)
        {
            npc.currentWaypointNumber = 0;
        }
        npc.currentWaypoint = npc.wayPoints[npc.currentWaypointNumber];
        ia.SetDestination(npc.wayPoints[npc.currentWaypointNumber].position);
    }
    public void Action()
    {
        if(npc.transform.position.x == npc.currentWaypoint.position.x && npc.transform.position.z == npc.currentWaypoint.position.z)
        {
            Debug.Log("Cheguei");
            if(Pesquisar(npc.currentWaypoint))
            {
               npc.MudarComportamento(new Stop(npc, ia));
            }
            else
            {
                Debug.Log("J");
                npc.MudarComportamento(new Mover(npc, ia));
            }
        }
    }
    bool Pesquisar(Transform t)
    {
        for (int m = 0; m < npc.stopPoints.Length; m++)
        {
            if (t == npc.stopPoints[m]) return true;
        }
        return false;
    }
}
