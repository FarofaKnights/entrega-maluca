using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSubject : MonoBehaviour {
    public System.Action<Collider> onTriggerEnter;
    public System.Action<Collider> onTriggerExit;
    public System.Action<Collider> onTriggerStay;

    void OnTriggerEnter(Collider other) {
        if (onTriggerEnter != null) onTriggerEnter(other);
    }

    void OnTriggerExit(Collider other) {
        if (onTriggerExit != null) onTriggerExit(other);
    }

    void OnTriggerStay(Collider other) {
        if (onTriggerStay != null) onTriggerStay(other);
    }
}
