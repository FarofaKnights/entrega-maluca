using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public enum Estado { Jogando, Pausado };
    public Estado estadoAtual = Estado.Jogando;

    float timeScaleAntigo = 1;
    Controls controls;
    public GameObject prefabGenerico; // acho que ninguém usa isso mais mas to com medo de tirar

    void Awake() {
        instance = this;
        controls = new Controls();
        controls.Game.Pausar.performed += ctx => TogglePause();
    }

    void OnEnable() {
        controls.Game.Enable();
    }

    void OnDisable() {
        controls.Game.Disable();
    }

    public void TogglePause() {
        if (estadoAtual == Estado.Jogando) {
            Pausar();
        } else {
            Despausar();
        }
    }

    public void Pausar() {
        estadoAtual = Estado.Pausado;
        timeScaleAntigo = Time.timeScale;
        Time.timeScale = 0;

        UIController.pause.Mostrar();
    }

    public void Despausar() {
        estadoAtual = Estado.Jogando;
        Time.timeScale = timeScaleAntigo;

        UIController.pause.Esconder();
    }

    public void VoltarMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
