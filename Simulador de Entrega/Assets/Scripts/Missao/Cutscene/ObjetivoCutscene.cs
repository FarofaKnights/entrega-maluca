using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneGroup {
    public ObjetivoCutscene inicial;
    public ObjetivoCutscene final;

    public CutsceneGroup(ObjetivoCutscene inicial, ObjetivoCutscene final) {
        this.inicial = inicial;
        this.final = final;
    }

    public CutsceneGroup(Missao missao) {
        Cutscene cutsInicial = missao.info.GetCutsceneInicial();
        if (cutsInicial != null) {
            inicial = new ObjetivoCutscene(cutsInicial);
            inicial.SetCargas(missao.GetObjetivoInicial().RemoveCargas());
            inicial.enderecoCutscene = missao.GetObjetivoInicial().endereco;
        }

        Cutscene cutsFinal = missao.info.GetCutsceneConclusao();
        if (cutsFinal != null) {
            final = new ObjetivoCutscene(cutsFinal);
            final.enderecoCutscene = missao.GetObjetivoInicial().endereco; // eu tinha feito pra pegar o endereço final mas não tem pq o personagem estar no endereço final
        }
    }
}

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
