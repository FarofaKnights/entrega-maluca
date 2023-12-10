using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Personagem Dialogo", menuName = "Entrega Maluca/Dialogo"), System.Serializable]

public class DialogoPersonagens : ScriptableObject
{
    public  FalaPersonagens falaInicial;
    public FalaPersonagens falaConclusao;


}

