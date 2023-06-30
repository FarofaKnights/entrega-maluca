using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public enum Estado { Jogando, Pausado };
    public Estado estadoAtual = Estado.Jogando;

    float timeScaleAntigo = 1;

    public GameObject prefabGenerico;
    void Awake() {
        instance = this;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (estadoAtual == Estado.Jogando) {
                Pausar();
            } else {
                Despausar();
            }
        }
    }

    public void Pausar() {
        estadoAtual = Estado.Pausado;
        timeScaleAntigo = Time.timeScale;
        Time.timeScale = 0;

        UIController.instance.EntrarPausa();
    }

    public void Despausar() {
        estadoAtual = Estado.Jogando;
        Time.timeScale = timeScaleAntigo;

        UIController.instance.SairPausa();
    }

    public void VoltarMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
