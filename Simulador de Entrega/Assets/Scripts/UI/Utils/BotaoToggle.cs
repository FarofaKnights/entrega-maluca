using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotaoToggle : MonoBehaviour {

    Toggle toggle;

    [Range(0f,2f), SerializeField] float escala = 1.1f;
    [SerializeField] float tempoAnimacao = 0.1f;

    void Start()  {
        HandleChange();
    }

    public void UpdateValues() {
        toggle = GetComponent<Toggle>();
    }

    public void HandleChange() {
        if (toggle == null) UpdateValues();

        if (toggle.isOn) {
            Selecionar();
        } else {
            Deselecionar();
        }
        
    }

    void Selecionar() {
        StartCoroutine(Animations.UnscaledAnimation(tempoAnimacao, (float t) => {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * escala, t);
        }));
    }

    void Deselecionar() {
        StartCoroutine(Animations.UnscaledAnimation(tempoAnimacao, (float t) => {
            transform.localScale = Vector3.Lerp(Vector3.one * escala, Vector3.one, t);
        }));
    }
}
