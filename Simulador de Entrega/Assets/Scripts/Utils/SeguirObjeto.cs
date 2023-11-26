using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirObjeto : MonoBehaviour {
    public Transform objeto;

    public void SetObjeto(Transform objeto) {
        this.objeto = objeto;

        if (objeto != null)
            transform.position = objeto.position;
    }

    void FixedUpdate() {
        if (objeto == null) return;
        transform.position = objeto.position;
    }
}
