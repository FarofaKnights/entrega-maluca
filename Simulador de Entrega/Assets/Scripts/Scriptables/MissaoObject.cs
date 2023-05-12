using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Missao", menuName = "Missao")]
public class MissaoObject : ScriptableObject {
    public string nome;
    public string descricao;

    public ObjetivoObject objetivoInicial;

    public ConjuntoObject[] conjuntos;
}
