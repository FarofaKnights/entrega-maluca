using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixa : MonoBehaviour {
    public Carga carga;
    
    [HideInInspector] public Quaternion rotacaoInicial;
    [HideInInspector] public TriggerSubject trigger;
    [HideInInspector] public CollisionSubject collision;

    IState estadoAtual;

    // temp: ideia de criar um singleton pra isso, uma vez que segundo o roque, flyweight não abrange esse tipo de coisa
    [SerializeField] GameObject clashEffect;
    AudioSource baterSom;

    void Start() {
        rotacaoInicial = transform.rotation;
        baterSom = GetComponent<AudioSource>();

        trigger = gameObject.AddComponent<TriggerSubject>();
        collision = gameObject.AddComponent<CollisionSubject>();
        
        trigger.Desativar();
    }

    void FixedUpdate() {
        estadoAtual?.Execute(Time.fixedDeltaTime);
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
        GameObject explosion = Instantiate(clashEffect, transform.position, transform.rotation);
        Destroy(explosion, 0.75f);

        carga.Destruida();

        Player.instance.RemoverCarga(carga);
        Destroy(gameObject);
    }

    public void BarulhoBater() {
        baterSom.Play();
    }
}
