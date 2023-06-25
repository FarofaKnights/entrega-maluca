using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cutscene {
    public string nome;
    public string texto;
    public Sprite imagem;
    public Cutscene proximaCutscene;

    public Cutscene(string nome, string texto, Sprite imagem) {
        this.nome = nome;
        this.texto = texto;
        this.imagem = imagem;
        this.proximaCutscene = null;
    }
}
