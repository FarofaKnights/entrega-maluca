using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissaoData
{
    public List<Missao> missoesConcluidas;
    public List<Missao> missoesDisponiveis;
    public Missao missaoAtual;
    public MissaoData (List<Missao> mc, List<Missao> md, Missao ma)
    {
        this.missoesConcluidas = mc;
        this.missoesDisponiveis = md;
        this.missaoAtual = ma;
    }
    

}
