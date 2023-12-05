using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData
{
    public string[] compradosNomes;
    public string[] ativosNomes;
   public UpgradeData(string [] comprados, string[] ativos)
    {
        this.compradosNomes = comprados;
        this.ativosNomes = ativos;
    }
}
