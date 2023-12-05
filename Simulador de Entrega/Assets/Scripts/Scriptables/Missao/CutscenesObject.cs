using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenesObject : ObjetivoObject {
    public CutsceneObject[] cutscenes;

    public override Objetivo Convert() {
        Cutscene[] cutscenes = new Cutscene[this.cutscenes.Length];
        for (int i = 0; i < this.cutscenes.Length; i++) {
            cutscenes[i] = this.cutscenes[i].Convert();
        }
        return new ObjetivoCutscene(cutscenes);
    }
}
