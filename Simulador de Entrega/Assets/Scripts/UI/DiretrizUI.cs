using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TextoDiretrizInfo {
    public float tamanhoTab;
    public int tamanhoFonte;
}

public class DiretrizUI : MonoBehaviour {
    public float tempoPopup = 4f;
    public Text titulo;
    public Image portrait;
    public GameObject diretrizItemPrefab;
    public GameObject diretrizItemHolder;
    public GameObject btnsHolder;

    protected TextoDiretriz itemConjunto;
    protected List<TextoDiretriz> itensObjetivo = new List<TextoDiretriz>();

    public GameObject informativoVazioPrefab, informativoPopup;
    GameObject informativoVazio;

    public TextoDiretrizInfo infoNivel1, infoNivel2;
    Missao missaoAtual;

    public Animator animator;
    bool closing = false;

    public void SetCurrentMissao(Missao missao) {
        Clear();

        missaoAtual = missao;
        titulo.text = missao.info.nome;

        informativoVazio = Instantiate(informativoVazioPrefab, diretrizItemHolder.transform);
        informativoVazio.transform.SetAsFirstSibling();

        if (missao.info.personagem != null) portrait.sprite = missao.info.personagem.portrait;
        else {
            portrait.sprite = null;
            Debug.LogError("Missão atual sem personagem!?!?!?!?!?");
        }
    }

    void Clear() {
        titulo.text = "";
        portrait.sprite = null;
        
        foreach (Transform child in diretrizItemHolder.transform) {
            if (child.gameObject == informativoPopup) continue;
            Destroy(child.gameObject);
        }

        itemConjunto = null;
        itensObjetivo.Clear();
    }

    public void Close(Missao missao) {
        if (missaoAtual != missao) return;
        
        missaoAtual = null;
        Clear();
        Hide();
    }

    public void AddDiretriz(Diretriz diretriz, int nivel) {
        GameObject diretrizItem = Instantiate(diretrizItemPrefab, transform);
        TextoDiretriz texto = diretrizItem.GetComponent<TextoDiretriz>();
        texto.SetDiretriz(diretriz);

        if (informativoVazio != null) {
            Destroy(informativoVazio);
            informativoVazio = null;
        }


        // Se conjunto
        if (nivel == 1) {
            foreach (TextoDiretriz item in itensObjetivo) {
                Destroy(item.gameObject);
            }
            itensObjetivo.Clear();
            if (itemConjunto != null) itemConjunto.SetDestaque(false);
            itemConjunto = diretrizItem.GetComponent<TextoDiretriz>();
        } else {
            itensObjetivo.Add(diretrizItem.GetComponent<TextoDiretriz>());
        }

        diretrizItem.transform.SetParent(diretrizItemHolder.transform);

        ShowPopup(diretriz);
    }

    public void ConcluirDiretriz(Diretriz diretriz) {
        if (itemConjunto != null && diretriz == itemConjunto.GetDiretriz()) {
            itemConjunto.SetRiscado(true);
        } else {
            foreach (TextoDiretriz item in itensObjetivo) {
                if (item.GetDiretriz() == diretriz) {
                    item.SetRiscado(true);
                    break;
                }
            }
        }
    }

    public void Show() {
        if (missaoAtual == null) return;

        foreach (Transform child in diretrizItemHolder.transform) {
            if (child.GetComponent<TextoDiretriz>() == null) continue;
            child.gameObject.SetActive(true);
        }

        informativoPopup.SetActive(false);
        btnsHolder.SetActive(true);
        gameObject.SetActive(true);
        animator.SetTrigger("QuickShow");
        closing = false;
    }

    public void Hide() {
        if (!gameObject.activeSelf) return;
        animator.SetTrigger("QuickHide");
        gameObject.SetActive(false);
    }

    public void ShowPopup(Diretriz diretriz) {
        if (gameObject.activeSelf) return;

        informativoPopup.SetActive(true);

        gameObject.SetActive(true);
        btnsHolder.SetActive(false);

        foreach (Transform child in diretrizItemHolder.transform) {
            TextoDiretriz texto = child.GetComponent<TextoDiretriz>();
            if(texto == null) continue;

            if (texto.GetDiretriz() == diretriz || texto == itemConjunto) {
                texto.gameObject.SetActive(true);
            } else {
                texto.gameObject.SetActive(false);
            }
        }

        animator.SetTrigger("Show");
        StartCoroutine(HidePopup());
        closing = false;
    }

    IEnumerator HidePopup() {
        yield return new WaitForSeconds(tempoPopup);
        animator.SetTrigger("Hide");
        closing = true;
    }

    public void HandleClosing() {
        if (closing) {
            closing = false;
            gameObject.SetActive(false);
            informativoPopup.SetActive(false);
        }
    }
}
