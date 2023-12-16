using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public enum Estado { Jogando, Pausado };
    public Estado estadoAtual = Estado.Jogando;

    float timeScaleAntigo = 1;
    Controls controls;

    public GameObject cutsceneVirtualCamera;
    public LayerMask caixaLayer;

    void Awake() {
        instance = this;
        controls = new Controls();
        controls.Game.Pausar.performed += ctx => TogglePause();
    }

    void FixedUpdate() {
        // Se 1, 9 e 0 estão pressionados, ativa os cheats
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) {
            if (Input.GetKey(KeyCode.Keypad9) || Input.GetKey(KeyCode.Alpha9)) {
                if (Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Alpha1)) {
                   EnableCheats();
                }
            }
        }
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

    public void VirtualCameraToMiddleOf(GameObject obj) {
        GameObject cabeca = obj.GetComponent<Personagem>().cabecaPos;

        CinemachineVirtualCamera cineMachine = cutsceneVirtualCamera.GetComponent<CinemachineVirtualCamera>();
        cineMachine.enabled = true;
        cineMachine.LookAt = cabeca.transform;

        cutsceneVirtualCamera.transform.position = cabeca.transform.position;

        cutsceneVirtualCamera.transform.position += cutsceneVirtualCamera.transform.forward * -3;
    }

    public void HideVirtualCamera() {
        CinemachineVirtualCamera cineMachine = cutsceneVirtualCamera.GetComponent<CinemachineVirtualCamera>();
        cineMachine.enabled = false;

    }

    #region Cheats

    void EnableCheats() {
        controls.Cheat.Enable();
        controls.Cheat.TP.performed += ctx => TPCheat();
        controls.Cheat.Dinheiro.performed += ctx => DinheiroCheat();
        controls.Cheat.Sair.performed += ctx => DisableCheats();
        UIController.instance.ShowCheatsIcon();
        Debug.Log("Cheats enabled");
    }

    void DisableCheats() {
        controls.Cheat.Disable();
        UIController.instance.HideCheatsIcon();
        Debug.Log("Cheats disabled");
    }

    void TPCheat() {
        if (MissaoManager.instance.missaoAtual == null) return;

        List<Carga> cargas = Player.instance.cargaAtual;
        List<(Carga, Transform)> cargasTransforms = new List<(Carga, Transform)>();

        foreach (Carga carga in cargas) {
            if (carga.cx != null) {
                cargasTransforms.Add((carga, carga.cx.transform));
                carga.cx.transform.SetParent(Player.instance.transform);
            }
        }

        Objetivo objetivo = MissaoManager.instance.GetCurrentObjetivo();
        Endereco endereco = objetivo.endereco;
        endereco.TeleportToHere();

        foreach (Carga carga in cargas) {
            if (carga.cx != null) {
                foreach ((Carga, Transform) cargaTransform in cargasTransforms) {
                    if (cargaTransform.Item1 == carga) {
                        carga.cx.transform.SetParent(cargaTransform.Item2);
                    }
                }
            }
        }
    }

    void DinheiroCheat() {
        Player.instance.AdicionarDinheiro(100);
    }

    #endregion
}
