using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SairTutorial : MonoBehaviour {
    public static SairTutorial instance;
    public string nomeCena, ultimoEndereco;
    public bool terminouTutorial = false;

    public GameObject confirmacao;

    void Awake() {
        instance = this;

        Time.timeScale = 0;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (terminouTutorial) HandleSairTutorial();
            else {
                confirmacao.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    void FixedUpdate() {
        if (!terminouTutorial && Player.instance.objetivosAtivos.Count > 0 && Player.instance.objetivosAtivos[0].endereco.nome == ultimoEndereco) {
            terminouTutorial = true;
        }
    }

    public void HandleSairTutorial() {
        Time.timeScale = 1;
        SceneManager.LoadScene(nomeCena);
    }

    public void CloseConfirmation() {
        Time.timeScale = 1;
        confirmacao.SetActive(false);
    }
}
