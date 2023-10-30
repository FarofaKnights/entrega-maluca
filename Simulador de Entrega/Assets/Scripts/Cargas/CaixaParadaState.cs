using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaParadaState : IState {
    Caixa caixa;

    public GameObject gameObject => caixa.gameObject;
    public Transform transform => caixa.transform;

    public CaixaParadaState(Caixa caixa) {
        this.caixa = caixa;
    }

    public void Enter() {
    }

    public void Execute(float dt) {}
    public void Exit() {
    }

    private void OnCollisionEnter(Collision collision) {
        if (Player.instance.GetState() is EncaixeState) return;
        if (collision.gameObject.name == "Veiculo" || collision.gameObject.tag == gameObject.tag) return;

        float velocity = caixa.GetComponent<Rigidbody>().velocity.magnitude;
        caixa.carga.fragilidade -= velocity;
        // caixa.carga.dentroCarro = false; //trocar estado
        // bater.Play();
        // MissaoManager.instance.missaoAtual.CargaRemove(this.carga);
        if (caixa.carga.fragilidade <= 0) {
            caixa.Explodir();
        }
    }
}
