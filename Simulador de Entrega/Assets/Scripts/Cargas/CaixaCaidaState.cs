using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaCaidaState : IState {
    Caixa caixa;

    public GameObject gameObject => caixa.gameObject;
    public Transform transform => caixa.transform;

    public CaixaCaidaState(Caixa caixa) {
        this.caixa = caixa;
    }

    public void Enter() {
        caixa.trigger.onTriggerEnter += OnTriggerEnter;
        caixa.trigger.onTriggerEnter += OnTriggerEnter;
        caixa.trigger.onTriggerStay += OnTriggerStay;

        Player.instance.RemoverCarga(caixa.carga);
    }

    public void Execute(float dt) { }
    public void Exit() {
        caixa.trigger.onTriggerEnter -= OnTriggerEnter;
        caixa.trigger.onTriggerEnter -= OnTriggerEnter;
        caixa.trigger.onTriggerStay -= OnTriggerStay;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name != "Veiculo") return;

        Player.instance.AdicionarCargaProxima(caixa.carga);
        UIController.HUD.MostrarBotaoRecuperar(true);
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name != "Veiculo") return;

        // StartCoroutine("WaitToRemove");
        Player.instance.RemoverCargaProxima(caixa.carga);
        UIController.HUD.MostrarBotaoRecuperar(false);
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.name != "Veiculo") return;

        UIController.HUD.MostrarBotaoRecuperar(true);
    }
    /*
    IEnumerator WaitToRemove() {
        yield return new WaitForSeconds(3f);
        
        Player.instance.RemoverCargaProxima(caixa.carga);
        UIController.HUD.MostrarBotaoRecuperar(false);
    }*/
}
