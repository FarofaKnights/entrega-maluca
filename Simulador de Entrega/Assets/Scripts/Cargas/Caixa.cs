using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixa : MonoBehaviour {
    public Carga carga;

    public Quaternion rotacaoInicial;

    IState estadoAtual;

    // temp: ideia usar um flyweight
    public GameObject Clash_Txt;

    void Start() {
        rotacaoInicial = transform.rotation;
    }


    public void SetState(IState estado) {
        estadoAtual?.Exit();
        estadoAtual = estado;
        estadoAtual.Enter();
    }

    public IState GetState() {
        return estadoAtual;
    }

    public CaixaEncaixeState GetEncaixeState() {
        if (estadoAtual is CaixaEncaixeState) 
            return (CaixaEncaixeState) estadoAtual;
        return null;
    }

    public void Explodir() {
        GameObject explosion = Instantiate(Clash_Txt, transform.position, transform.rotation);
        Destroy(explosion, 0.75f);

        Destroy(gameObject);
    }

    void OnDestroy() {
        // Fazer algo
    }
}
