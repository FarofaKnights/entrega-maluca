using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetivoCutscene : Objetivo {
    public Cutscene cutscene;
    public Endereco enderecoCutscene;

    public ObjetivoCutscene(FalaPersonagens fala, PersonagemObject personagem): base(MissaoManager.instance.enderecoFalso) {
        this.cutscene = new Cutscene(fala, personagem);
    }

    public ObjetivoCutscene(Cutscene cutscene): base(MissaoManager.instance.enderecoFalso) {
        this.cutscene = cutscene;
    }

    public ObjetivoCutscene(Cutscene[] cutscene): base(MissaoManager.instance.enderecoFalso) {
        this.cutscene = cutscene[0];

        for (int i = 0; i < cutscene.Length - 1; i++) {
            cutscene[i].proximaCutscene = cutscene[i + 1];
        }
    }

    public override void Iniciar() {
        base.Iniciar();

        if (enderecoCutscene != null) cutscene.Play(Concluir, enderecoCutscene);
        else cutscene.Play(Concluir);
    }

    public override void Parar() {
        cutscene.Parar();
        base.Parar();
    }

}
