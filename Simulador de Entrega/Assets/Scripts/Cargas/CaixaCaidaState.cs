using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CaixaCaidaState : IState {
    Caixa caixa;

    public GameObject gameObject => caixa.gameObject;
    public Transform transform => caixa.transform;

    GameObject indicador;
    Controls controls;

    public CaixaCaidaState(Caixa caixa) {
        this.caixa = caixa;
    }

    public void Enter() {
        CriarIndicador();

        caixa.trigger.Ativar();
        caixa.trigger.onTriggerEnter += OnTriggerEnter;
        caixa.trigger.onTriggerExit += OnTriggerExit;
        caixa.trigger.onTriggerStay += OnTriggerStay;

        controls = new Controls();
        controls.Game.Recuperar.performed += Recuperar;

        gameObject.layer = LayerMask.NameToLayer("Caida");
        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Caida");

        IncluiMinimapa icon = caixa.gameObject.AddComponent<IncluiMinimapa>();
        icon.sprite = MinimapManager.instance.caixaCaidaSprite;
        icon.comecaAtivo = true;
        icon.escala = 5f;
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

    void Recuperar(InputAction.CallbackContext callback) {
        Player.instance.RecuperarCargasProximas();
    }

    void CriarIndicador() {
        indicador = GameObject.Instantiate(GameManager.instance.caixaCaidaIndicador, caixa.transform.position, Quaternion.identity);
        indicador.transform.SetParent(caixa.transform);

        indicador.transform.localPosition = Vector3.zero;
        indicador.transform.localRotation = Quaternion.identity;
        indicador.transform.localScale = Vector3.one * 0.75f;

        indicador.transform.position += Vector3.up * 4f;
        indicador.SetActive(false);
    }

    public void Execute(float dt) {
        indicador.transform.position = caixa.transform.position + Vector3.up * 4f;
    }
    public void Exit() {
        caixa.trigger.Desativar();
        caixa.trigger.onTriggerEnter -= OnTriggerEnter;
        caixa.trigger.onTriggerExit -= OnTriggerExit;
        caixa.trigger.onTriggerStay -= OnTriggerStay;

        controls.Game.Recuperar.performed -= Recuperar;

        gameObject.layer = LayerMask.NameToLayer("Caixa");
        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Caixa");

        GameObject.Destroy(caixa.GetComponent<IncluiMinimapa>());
        GameObject.Destroy(indicador);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name != "Veiculo") return;

        Player.instance.AdicionarCargaProxima(caixa.carga);
        indicador.SetActive(true);
        controls.Game.Recuperar.Enable();
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name != "Veiculo") return;

        Player.instance.RemoverCargaProxima(caixa.carga);
        indicador.SetActive(false);
        controls.Game.Recuperar.Disable();
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.name != "Veiculo") return;

        indicador.SetActive(true);
    }
}
