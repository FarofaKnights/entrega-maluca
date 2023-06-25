using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneObject {
    public string nome;
    public string texto;
    public Sprite imagem;

    public Cutscene Convert() {
        return new Cutscene(nome, texto, imagem);
    }
}
