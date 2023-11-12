using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Campo Nome Valor", menuName = "Entrega Maluca/Nome Valor Config"), System.Serializable]
public class CampoNomeValorScriptable : ScriptableObject {
    public float tempoFadeIn = 0.3f;
    public float tempoShowValor = 0.75f;
    public float tempoJumpValor = 0.2f;
    public float scaleMultJump = 1.25f;
}
