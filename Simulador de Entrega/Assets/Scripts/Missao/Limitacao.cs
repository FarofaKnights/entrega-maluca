using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Limitacao: Iniciavel {
    public void Falhar();
}

[System.Serializable]
public class LimitacaoObject: ScriptableObject {}