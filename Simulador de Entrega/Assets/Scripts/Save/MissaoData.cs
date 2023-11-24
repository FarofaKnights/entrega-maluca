using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissaoData
{
    public string[] missoesConcluidas;
    public string[] missoesDisponiveis;
    public float[] concluValores;
    public MissaoData (string[] mc, string[] md, float [] cv)
    {
        this.missoesConcluidas = mc;
        this.missoesDisponiveis = md;
        this.concluValores = cv;
    }
    

}
