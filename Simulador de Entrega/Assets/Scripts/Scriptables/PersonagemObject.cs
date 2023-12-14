using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fulano de tal", menuName = "Entrega Maluca/Personagem"), System.Serializable]
public class PersonagemObject : ScriptableObject {
    public string nome;
    public Sprite portrait, portraitGrande;
    public GameObject prefab;
    public FalaPersonagens falaFalha;
}
