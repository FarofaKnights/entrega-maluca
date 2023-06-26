using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetivoCutscene : Objetivo {
    public Cutscene cutscene;

    public ObjetivoCutscene(Cutscene cutscene): base(CutsceneController.instance.enderecoFalso) {
        this.cutscene = cutscene;
    }

    public ObjetivoCutscene(Cutscene[] cutscene): base(CutsceneController.instance.enderecoFalso) {
        this.cutscene = cutscene[0];

        for (int i = 0; i < cutscene.Length - 1; i++) {
            cutscene[i].proximaCutscene = cutscene[i + 1];
        }
    }

    public override void Iniciar() {
        base.Iniciar();

        CutsceneController.instance.IniciarCutscene(cutscene);
        CutsceneController.instance.aoTerminar += Concluir;
    }

    public override void Concluir() {
        CutsceneController.instance.aoTerminar -= Concluir;
        base.Concluir();
    }

}
