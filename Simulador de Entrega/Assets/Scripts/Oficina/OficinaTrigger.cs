using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OficinaTrigger : MonoBehaviour {
    public OficinaController oficina;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            oficina.EntrarOficina();
        }
    }
}
