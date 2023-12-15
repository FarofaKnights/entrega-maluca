using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimacoesFala { Parado, Iritado, Conversar, Acenar, Andar }


[System.Serializable]
public class FalaPersonagens
{
    public string fala;
    public AudioClip audio;
    public AnimacoesFala animacao = AnimacoesFala.Conversar;
}
