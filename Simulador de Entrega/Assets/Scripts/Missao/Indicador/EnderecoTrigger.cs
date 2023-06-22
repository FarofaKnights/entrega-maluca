using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnderecoTrigger : MonoBehaviour {
    public Endereco endereco;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            endereco.HandleTrigger(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            endereco.HandleTrigger(false);
        }
    }
}
