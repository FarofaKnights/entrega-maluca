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
    public Text titulo;
    public Image portrait;
    public GameObject diretrizItemPrefab;
    public GameObject diretrizItemHolder;

    protected TextoDiretriz itemConjunto;
    protected List<TextoDiretriz> itensObjetivo = new List<TextoDiretriz>();

    public TextoDiretrizInfo infoNivel1, infoNivel2;
    Missao missaoAtual;

    public void SetCurrentMissao(Missao missao) {
        Clear();

        missaoAtual = missao;
        titulo.text = missao.info.nome;

        if (missao.info.personagem != null) portrait.sprite = missao.info.personagem.portrait;
        else {
            portrait.sprite = null;
            Debug.LogError("Missão atual sem personagem!?!?!?!?!?");
        }


        gameObject.SetActive(true);
    }

    void Clear() {
        titulo.text = "";
        portrait.sprite = null;
        
        foreach (Transform child in diretrizItemHolder.transform) {
            Destroy(child.gameObject);
        }

        itemConjunto = null;
        itensObjetivo.Clear();
    }

    public void Close(Missao missao) {
        if (missaoAtual != missao) return;
        
        missaoAtual = null;
        Clear();
        gameObject.SetActive(false);
    }

    public void AddDiretriz(Diretriz diretriz, int nivel) {
        GameObject diretrizItem = Instantiate(diretrizItemPrefab, transform);
        TextoDiretriz texto = diretrizItem.GetComponent<TextoDiretriz>();
        texto.SetDiretriz(diretriz);

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
}
