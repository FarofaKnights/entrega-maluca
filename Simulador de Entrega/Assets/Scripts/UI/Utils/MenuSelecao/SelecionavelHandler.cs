using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelecionavelHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

    [SerializeField] float movimentoVertical = 30f;
    [Range(0f,2f), SerializeField] float escala = 1.1f;
    [SerializeField] float tempoAnimacao = 0.1f;

    public Vector3 posicaoInicial;
    public Vector3 escalaInicial = Vector3.one;

    public System.Action onSelecionado, onDeselecionado;

    SelecionavelController controller {
        get { return GetComponentInParent<SelecionavelController>(); }
    }

    void Start() {
        posicaoInicial = transform.position;
    }

    public void Selecionar() {
        StartCoroutine(SelecionarAnimacao());
        
        if (controller != null) {
            controller.HandleSelecionado(gameObject);
        }

        if (onSelecionado != null) onSelecionado();
    }

    void OnDisable() {
        DeselecionarInstantanea();
    }

    public void Deselecionar() {
        StartCoroutine(DeselecionarAnimacao());
        
        if (onDeselecionado != null) onDeselecionado();
    }

    IEnumerator SelecionarAnimacao() {
        float tempo = 0f;
        while (tempo < tempoAnimacao) {
            tempo += Time.unscaledDeltaTime;
            if (movimentoVertical != 0) transform.position = Vector3.Lerp(posicaoInicial, posicaoInicial + Vector3.up * movimentoVertical, tempo / tempoAnimacao);
            transform.localScale = Vector3.Lerp(escalaInicial, escalaInicial * escala, tempo / tempoAnimacao);
            yield return null;
        }
    }

    IEnumerator DeselecionarAnimacao() {
        float tempo = 0f;
        while (tempo < tempoAnimacao) {
            tempo += Time.unscaledDeltaTime;
            if (movimentoVertical != 0) transform.position = Vector3.Lerp(posicaoInicial + Vector3.up * movimentoVertical, posicaoInicial, tempo / tempoAnimacao);
            transform.localScale = Vector3.Lerp(escalaInicial * escala, escalaInicial, tempo / tempoAnimacao);
            yield return null;
        }
    }

    public void DeselecionarInstantanea() {
        if (onDeselecionado != null) onDeselecionado();

        if (movimentoVertical != 0) transform.position = posicaoInicial;
        transform.localScale = escalaInicial;
    }
    
    public void OnPointerEnter(PointerEventData eventData) {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.selectedObject == gameObject) {
            eventData.selectedObject = null;
        }
    }

    public void OnSelect(BaseEventData eventData) {
        Selecionar();
    }

    public void OnDeselect(BaseEventData eventData) {
        Deselecionar();
    }

    
}
