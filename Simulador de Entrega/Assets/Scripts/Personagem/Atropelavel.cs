using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atropelavel : MonoBehaviour {
    public GameObject amassavel;
    public bool desamassarOnLeave = true;
    public float amassoTempo = 0.25f;
    public float dessamasoTempo = 0f;

    public System.Action OnAmassado, OnDesamassado;
    Vector3 escalaAmassada = new Vector3(1f, 0.1f, 1f);

    bool amassado = false;

    IEnumerator animacao;

    public void Amassar() {
        if (amassado) return;
        amassado = true;

        if (animacao != null) StopCoroutine(animacao);
        StopCoroutine("DesamassarApos");

        animacao = Animations.Animation(amassoTempo, (float t) => {
            amassavel.transform.localScale = Vector3.Lerp(Vector3.one, escalaAmassada, t);

            if (t >= 1) {
                if (OnAmassado != null) OnAmassado();
                animacao = null;
            }
        });
        StartCoroutine(animacao);
    }

    IEnumerator DesamassarApos() {
        yield return new WaitForSeconds(dessamasoTempo);
        Desamassar();
    }

    public void Desamassar() {
        if (!amassado) return;
        amassado = false;

        if (animacao != null) StopCoroutine(animacao);

        animacao = Animations.Animation(amassoTempo, (float t) => {
            amassavel.transform.localScale = Vector3.Lerp(escalaAmassada, Vector3.one, t);

            if (t >= 1) {
                if (OnDesamassado != null) OnDesamassado();
                animacao = null;
            }
        });
        StartCoroutine(animacao);
    }


    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Amassar();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (desamassarOnLeave) StartCoroutine("DesamassarApos");
        }
    }
}
