using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaEncaixeState : IState {
    Caixa caixa;
    Rigidbody rb;
    Transform spawnPoint;

    public GameObject gameObject => caixa.gameObject;
    public Transform transform => caixa.transform;

    public CaixaEncaixeState(Caixa caixa, Transform spawnPoint) {
        this.caixa = caixa;
        this.spawnPoint = spawnPoint;
    }

    public void Enter() {
        rb = caixa.GetComponent<Rigidbody>();
        caixa.transform.position = spawnPoint.position;
    }

    public void Execute(float dt) {}
    public void Exit() {

    }


    public void Selecionar() {
        if (rb == null) rb = caixa.GetComponent<Rigidbody>();

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
        caixa.transform.rotation = caixa.rotacaoInicial;
        caixa.transform.position = spawnPoint.position;
    }
}
