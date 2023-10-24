using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelecionavelController : MonoBehaviour {
    GameObject ultimoSelecionado;
    int index = 0;

    Controls controls;

    void Awake() {
        controls = new Controls();
        controls.UI.Navigate.performed += ctx => Navegar(ctx.ReadValue<Vector2>());
    }

    void OnEnable() {
        controls.UI.Enable();
        StartCoroutine(AposUmFrame());
    }

    void OnDisable() {
        controls.UI.Disable();
    }

    public GameObject[] GetSelecionaveis() {
        SelecionavelHandler[] selecionaveis = GetComponentsInChildren<SelecionavelHandler>();
        GameObject[] selecionaveisGO = new GameObject[selecionaveis.Length];
        for (int i = 0; i < selecionaveis.Length; i++) {
            selecionaveisGO[i] = selecionaveis[i].gameObject;
        }
        return selecionaveisGO;
    }

    public void Navegar(Vector2 vector2) {
        GameObject[] selecionaveis = GetSelecionaveis();
        foreach (GameObject selecionavel in selecionaveis) {
            // Se o objeto selecionado for um dos selecionáveis, não fazer nada
            if (EventSystem.current.currentSelectedGameObject == selecionavel) {
                ultimoSelecionado = selecionavel;
                return;
            }
        }

        index = Mathf.Clamp(index, 0, selecionaveis.Length - 1);

        if (index > 0 && selecionaveis.Length > 1)
            EventSystem.current.SetSelectedGameObject(selecionaveis[index]);
    }

    public void HandleSelecionado(GameObject selecionado) {
        ultimoSelecionado = selecionado;

        GameObject[] filhos = GetSelecionaveis();
        for (int i = 0; i < filhos.Length; i++) {
            if (filhos[i] == selecionado) {
                index = i;
                break;
            }
        }
    }

    IEnumerator AposUmFrame() {
        yield return null;
        GameObject primeiroSelecionavel = null;

        foreach (SelecionavelHandler filho in GetComponentsInChildren<SelecionavelHandler>()) {
            if (filho.gameObject.activeSelf) {
                primeiroSelecionavel = filho.gameObject;
                break;
            }
        }

        if (primeiroSelecionavel != null) {
            EventSystem.current.SetSelectedGameObject(primeiroSelecionavel);
        }
    }
}
