using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneUI : MonoBehaviour {
    public TextMeshProUGUI nome;
    public Text fala;
    public Image imagem;
    public GameObject nextButton;

    public System.Action OnNext;

    public void ShowFala(PersonagemObject personagem, FalaPersonagens fala, System.Action next) {
        nome.text = personagem.nome;
        imagem.sprite = personagem.portraitGrande;
        
        nextButton.SetActive(false);
        OnNext = next;

        StopCoroutine("TypeSentence");
        StartCoroutine(TypeSentence(fala.fala));

        Time.timeScale = 0f;
    }

    IEnumerator TypeSentence(string sentence) {
        fala.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            fala.text += letter;
            yield return null;
        }

        nextButton.SetActive(true);
    }

    public void HandleNext() {
        Time.timeScale = 1f;
        gameObject.SetActive(false);

        if (OnNext != null){
            OnNext();
            OnNext = null;
        }
            
    }
}
