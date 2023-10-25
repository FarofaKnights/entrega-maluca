using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : ComportamentoCarros
{
    CarrosNPC npc;
    NavMeshAgent ia;
    public Mover(CarrosNPC carro, NavMeshAgent agent)
    {
        npc = carro;
        ia = agent;
    }
    public void Enter()
    {
        int nextWaypoint = Random.Range(0, WayPointCarrosManager.instance.wayPoints.Length);
        while (WayPointCarrosManager.instance.wayPoints[nextWaypoint] == npc.currentWaypoint)
        {
            nextWaypoint = Random.Range(0, WayPointCarrosManager.instance.wayPoints.Length);
        }
        npc.currentWaypoint = WayPointCarrosManager.instance.wayPoints[nextWaypoint];
        ia.SetDestination(npc.currentWaypoint.position);
    }
    public void Action()
    {
        if(npc.transform.position.x == npc.currentWaypoint.position.x && npc.transform.position.z == npc.currentWaypoint.position.z)
        {
            if(Pesquisar(npc.currentWaypoint))
            {
               npc.MudarComportamento(new Stop(npc, ia));
            }
            else
            {
                npc.MudarComportamento(new Mover(npc, ia));
            }
        }
    }
    bool Pesquisar(Transform t)
    {
        for (int m = 0; m < WayPointCarrosManager.instance.stopPoints.Length; m++)
        {
            if (t == WayPointCarrosManager.instance.stopPoints[m]) return true;
        }
        return false;
    }
}
