using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour {
    public static CutsceneController instance;

    public Text nome, texto;
    public Image imagem;

    public Cutscene cutsceneAtual;

    public Endereco enderecoFalso;

    // Event holder
    public delegate void TerminouDelegate();
    public event TerminouDelegate aoTerminar;

    
    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        enderecoFalso = GetComponent<EnderecoFalso>();
    }

    void OnEnable() {
        Debug.Log(cutsceneAtual);
        Debug.Log(gameObject.activeSelf);
        if (cutsceneAtual != null) {
            UpdateValues();
        } else {
            Debug.Log("desativando");
            gameObject.SetActive(false);
        }
    }

    void OnDisable() {
        if (aoTerminar != null) {
            aoTerminar();
        }
    }

    public void IniciarCutscene(Cutscene cutscene) {
        cutsceneAtual = cutscene;

        Time.timeScale = 0;

        if (gameObject.activeSelf) {
            UpdateValues();
        } else {
            gameObject.SetActive(true);
        }
    }

    public void UpdateValues() {
        nome.text = cutsceneAtual.nome;
        texto.text = cutsceneAtual.texto;

        if (cutsceneAtual.imagem != null){
            imagem.sprite = cutsceneAtual.imagem;
            imagem.gameObject.SetActive(true);
        } else {
            imagem.gameObject.SetActive(false);
        }
    }

    public void HandleProximo() {
        Debug.Log("Proximo");
        if (cutsceneAtual.proximaCutscene != null) {
            IniciarCutscene(cutsceneAtual.proximaCutscene);
        } else {
            cutsceneAtual = null;
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}
