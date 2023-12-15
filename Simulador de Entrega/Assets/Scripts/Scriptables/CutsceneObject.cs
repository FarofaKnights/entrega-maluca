using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneObject {
    public FalaPersonagens fala;
    public PersonagemObject personagem;

    public Cutscene Convert() {
        return new Cutscene(fala, personagem);
    }
}
