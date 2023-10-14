using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spin : ComportamentoCarros
{
    CarrosNPC npc;
    NavMeshAgent ia;
    private float time = 0;

    public Spin(CarrosNPC carro, NavMeshAgent agent)
    {
        npc = carro;
        ia = agent;
    }

    public void Enter()
    {
        time = Time.time + 3;
    }

    public void Action()
    {
        if (Time.time > time)
        {
            npc.MudarComportamento(new Mover(npc, ia));
        }
        npc.transform.Rotate(0, 720 * Time.fixedDeltaTime, 0);
    }
}
