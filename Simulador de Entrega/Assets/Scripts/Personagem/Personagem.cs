using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour {
    public enum Estado {Parado, Aguardando, Acenando, Irritado, Conversando}
    public Animator animator;
    public Estado estado = Estado.Parado, lastEstado = Estado.Parado;
    public GameObject cabecaPos;
    public float aguardoArea = 1f;

    public void Aguardar() {
        SetEstado(Estado.Aguardando);
    }

    public void Conversar() {
        SetEstado(Estado.Conversando);
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
                animator.SetTrigger("Conversar");
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

    public static Personagem.Estado GetEstado(AnimacoesFala fala) {
        switch (fala) {
            case AnimacoesFala.Parado:
                return Personagem.Estado.Parado;
            case AnimacoesFala.Iritado:
                return Personagem.Estado.Irritado;
            case AnimacoesFala.Conversar:
                return Personagem.Estado.Conversando;
            case AnimacoesFala.Acenar:
                return Personagem.Estado.Acenando;
            case AnimacoesFala.Andar:
                return Personagem.Estado.Parado;
            default:
                return Personagem.Estado.Parado;
        }
    }
}
