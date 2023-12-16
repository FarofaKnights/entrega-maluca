using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cutscene {
    public FalaPersonagens fala;
    public PersonagemObject personagem;
    public Cutscene proximaCutscene;
    Endereco enderecoRef;
    System.Action nextRef;

    public Cutscene(FalaPersonagens fala, PersonagemObject personagem) {
        this.fala = fala;
        this.personagem = personagem;
        this.proximaCutscene = null;
    }

    public void Play(System.Action next) {
        UIController.instance.ShowCutscene(this, next);
    }

    public void Play() {
        UIController.instance.ShowCutscene(this, null);
    }

    public void Play(System.Action next, Endereco endereco) {
        if (endereco == null) {
            Play(next);
            return;
        }

        enderecoRef = endereco;
        nextRef = next;

        UIController.instance.ShowCutscene(this, HandleEndereco);

        if (!endereco.gameObject.activeSelf) endereco.gameObject.SetActive(true);

        GameObject obj = endereco.SetPersonagem(personagem);
        GameManager.instance.VirtualCameraToMiddleOf(obj);
        endereco.PersonagemSecundaryTarget(GameManager.instance.cutsceneVirtualCamera.transform);
        obj.GetComponent<Personagem>().SetEstado(Personagem.GetEstado(fala.animacao));

    }

    void HandleEndereco() {
        enderecoRef.RemovePersonagem();
        enderecoRef.gameObject.SetActive(false);
        GameManager.instance.HideVirtualCamera();
        enderecoRef.PersonagemSecundaryTarget(null);
        enderecoRef = null;

        if (nextRef != null)
            nextRef();

        nextRef = null;
    }

    public void Parar() {
        if (enderecoRef != null) {
            enderecoRef.RemovePersonagem();
            enderecoRef.gameObject.SetActive(false);
            GameManager.instance.HideVirtualCamera();
            enderecoRef.PersonagemSecundaryTarget(null);
        }

        UIController.cutscene.Esconder();
    }
}
