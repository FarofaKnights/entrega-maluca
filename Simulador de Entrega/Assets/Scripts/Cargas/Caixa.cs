using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixa : MonoBehaviour {
    public Carga carga;
    public Transform spawnPoint;
    Quaternion rotacaoInicial;

    Rigidbody rb;

    // temp: ideia usar um flyweight
    public GameObject Clash_Txt;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rotacaoInicial = transform.rotation;
    }

    public void Selecionar() {
        if (rb == null) rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Deselecionar() {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
    }

    public void Rodar(bool rodar) {
        if (rodar) {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        } else {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void SetVelocity(Vector3 velocity) {
        rb.velocity = velocity;
    }

    public void MoveDeltaRotation(Quaternion delta) {
        rb.MoveRotation(delta * rb.rotation);
    }

    public void ResetarPosicao() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Rodar(false);
        transform.rotation = rotacaoInicial;
        transform.position = spawnPoint.position;
    }

    public void Destruir() {
        // Nota de problema: a explosão só deve ocorrer na destruição por impacto, neste caso a explosão está acontecendo sempre que ela é destruida
        /*GameObject explosion = Instantiate(Clash_Txt, transform.position, transform.rotation);
        Destroy(explosion, 0.75f);*/

        Destroy(gameObject);
    }
}
