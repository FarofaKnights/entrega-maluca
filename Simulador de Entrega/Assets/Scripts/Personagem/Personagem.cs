using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour {
    public enum Estado {Parado, Aguardando, Acenando, Irritado, Conversando}
    public Animator animator;
    public Estado estado = Estado.Parado, lastEstado = Estado.Parado;
    public float aguardoArea = 1f;

    public void Aguardar() {
        SetEstado(Estado.Aguardando);
    }

    public void SetEstado(Estado estado) {
        lastEstado = this.estado;
        this.estado = estado;

        switch (estado) {
            case Estado.Parado:
                animator.SetTrigger("Parado");
                break;
            case Estado.Aguardando:
                animator.SetTrigger("Parado");
                break;
            case Estado.Irritado:
                animator.SetTrigger("Irritado");
                break;
            case Estado.Acenando:
                animator.SetTrigger("Acenar");
                break;
            case Estado.Conversando:
                animator.SetTrigger("Falar");
                break;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (estado == Estado.Aguardando) {
                SetEstado(Estado.Acenando);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            if (estado == Estado.Acenando) {
                SetEstado(Estado.Aguardando);
            }
        }
    }
}
