using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cutscene {
    public FalaPersonagens fala;
    public PersonagemObject personagem;
    public Cutscene proximaCutscene;

    public Cutscene(FalaPersonagens fala, PersonagemObject personagem) {
        this.fala = fala;
        this.personagem = personagem;
        this.proximaCutscene = null;
    }
}
