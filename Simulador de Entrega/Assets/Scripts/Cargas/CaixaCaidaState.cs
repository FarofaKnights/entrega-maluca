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
        caixa.trigger.Ativar();
        caixa.trigger.onTriggerEnter += OnTriggerEnter;
        caixa.trigger.onTriggerExit += OnTriggerExit;
        caixa.trigger.onTriggerStay += OnTriggerStay;

        IncluiMinimapa icon = caixa.gameObject.AddComponent<IncluiMinimapa>();
        icon.sprite = MinimapManager.instance.caixaCaidaSprite;
        icon.comecaAtivo = true;
        icon.escala = 3f;
        icon.SetColor(MinimapManager.instance.caixaCaidaColor);

        // Se o player já estiver no trigger, o OnTriggerStay não é chamado
        // Então temos que manualmente checar se o player está no trigger
        foreach (Collider col in caixa.trigger.GetTriggers()) {
            if (col is SphereCollider) {
                SphereCollider sphere = (SphereCollider) col;
                RaycastHit[] hits = Physics.SphereCastAll(sphere.transform.position, sphere.radius, Vector3.up, 0f);
                foreach (RaycastHit hit in hits) {
                    OnTriggerEnter(hit.collider);
                }
            }
        }

        Player.instance.RemoverCarga(caixa.carga);
    }

    public void Execute(float dt) { }
    public void Exit() {
        caixa.trigger.Desativar();
        caixa.trigger.onTriggerEnter -= OnTriggerEnter;
        caixa.trigger.onTriggerExit -= OnTriggerExit;
        caixa.trigger.onTriggerStay -= OnTriggerStay;

        GameObject.Destroy(caixa.GetComponent<IncluiMinimapa>());
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
