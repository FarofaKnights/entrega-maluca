using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneUI : MonoBehaviour {
    public Tela tela;

    public TextMeshProUGUI nome;
    public Text fala;
    public Image imagem;
    public GameObject nextButton;

    public System.Action OnNext;

    bool typing = false;
    string sentence = "";
    IEnumerator currentWriting = null;

    public void ShowCutscene(Cutscene cutscene, System.Action nextTrue) {
        System.Action next = () => {
            if (cutscene.proximaCutscene != null) {
                ShowCutscene(cutscene.proximaCutscene, nextTrue);
            } else {
                OnNext = nextTrue;
                HandleNext();
            }
        };

        ShowFala(cutscene.personagem, cutscene.fala, next);
    }


    public void ShowFala(PersonagemObject personagem, FalaPersonagens fala, System.Action next) {
        gameObject.SetActive(true);

        nome.text = personagem.nome;
        imagem.sprite = personagem.portraitGrande;
        
        OnNext = next;

        Debug.Log(fala.fala);
        if (currentWriting != null) StopCoroutine(currentWriting);
        currentWriting = TypeSentence(fala.fala);
        StartCoroutine(currentWriting);
    }

    IEnumerator TypeSentence(string sentence) {
        this.sentence = sentence;
        fala.text = "";
        typing = true;
        foreach (char letter in sentence.ToCharArray()) {
            fala.text += letter;

            if (letter == '.' || letter == '!' || letter == '?') {
                yield return new WaitForSecondsRealtime(0.2f);
            } else yield return null;
        }
        typing = false;
    }

    void ForceSentence() {
        if(!typing) return;

        fala.text = sentence;
        typing = false;
    }

    public void HandleNext() {
        if (typing) {
            if (currentWriting != null) StopCoroutine(currentWriting);
            ForceSentence();
            return;
        }

        Esconder();

        if (OnNext != null){
            OnNext();
            OnNext = null;
        }
            
    }

    public void Mostrar() {
        tela.Mostrar();
    }

    public void Esconder() {
        tela.Esconder();
        tela.GetVizinho("HUD")?.Mostrar();
    }
}
