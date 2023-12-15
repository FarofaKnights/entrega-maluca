using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetivoCutscene : Objetivo {
    public Cutscene cutscene;
    public Endereco enderecoCutscene;

    public ObjetivoCutscene(MissaoObject missao): base(MissaoManager.instance.enderecoFalso) {
        this.cutscene = new Cutscene(missao.dialogo.falaInicial, missao.personagem);
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
        UIController.instance.ShowCutscene(cutscene, Concluir);

        if (enderecoCutscene != null) {
            if (!enderecoCutscene.gameObject.activeSelf) enderecoCutscene.gameObject.SetActive(true);

            GameObject obj = enderecoCutscene.SetPersonagem(cutscene.personagem);
            GameManager.instance.VirtualCameraToMiddleOf(obj);
            enderecoCutscene.PersonagemSecundaryTarget(GameManager.instance.cutsceneVirtualCamera.transform);
            obj.GetComponent<Personagem>().Conversar();
        }
    }

    public override void Concluir() {
        if (enderecoCutscene != null) {
            enderecoCutscene.RemovePersonagem();
            enderecoCutscene.gameObject.SetActive(false);
            GameManager.instance.HideVirtualCamera();
            enderecoCutscene.PersonagemSecundaryTarget(null);
        }

        base.Concluir();
    }

    public override void Parar() {
        if (enderecoCutscene != null) {
            enderecoCutscene.RemovePersonagem();
            enderecoCutscene.gameObject.SetActive(false);
            GameManager.instance.HideVirtualCamera();
            enderecoCutscene.PersonagemSecundaryTarget(null);
        }

        UIController.cutscene.gameObject.SetActive(false);

        base.Parar();
    }

}
