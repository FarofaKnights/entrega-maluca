using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stop : ComportamentoCarros
{
    CarrosNPC npc;
    NavMeshAgent ia;
    private float time = 0;
    public Stop(CarrosNPC carro, NavMeshAgent agent)
    {
        npc = carro;
        ia = agent;
    }
    public void Enter()
    {
        time = Time.time + 3;
        ia.SetDestination(npc.transform.position);
    }
    public void Action()
    {
        if (Time.time > time)
        {
            int i = Random.Range(0, 101);
            if (i == 1)
            {
                npc.MudarComportamento(new Spin(npc, ia));
            }
            else
            {
                npc.MudarComportamento(new Mover(npc, ia));
            }
        }

    }
}
