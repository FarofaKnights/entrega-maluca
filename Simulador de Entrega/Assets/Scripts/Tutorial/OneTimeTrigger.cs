using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeTrigger : MonoBehaviour {
    public GameObject objeto;
    public bool estado;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            objeto.SetActive(estado);
            Destroy(gameObject);
        }
    }
}
