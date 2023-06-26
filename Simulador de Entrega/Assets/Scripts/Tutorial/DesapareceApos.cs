using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesapareceApos : MonoBehaviour {
    public float tempo;
    float tempoAtual = 0;

    // Start is called before the first frame update
    void Start() {
        tempoAtual = tempo;
    }

    void FixedUpdate() {
        tempoAtual -= Time.fixedDeltaTime;

        if (tempoAtual <= 0) {
            gameObject.SetActive(false);
        }
    }

}
