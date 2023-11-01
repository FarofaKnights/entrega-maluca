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
    }

    public void Execute(float dt) {
        // ChecarLimites();
    }
    public void Exit() {}

    public void ChecarLimites(){
        // O maior problema atualmente com esse método é que as caixas não são colocadas em nenhum container, então a posição local está bem diferente do esperado
        if (transform.localPosition.x > 7.2f || transform.localPosition.x < -7.2f || transform.localPosition.z < -7.5f || transform.localPosition.z > 0.3f || transform.localPosition.y > 7.5f || transform.localPosition.y < -1f) 
            ResetarPosicao();
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
