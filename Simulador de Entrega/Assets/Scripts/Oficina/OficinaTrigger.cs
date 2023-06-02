using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OficinaTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            transform.parent.GetComponent<OficinaController>().EntrarOficina();
        }
    }
}
