using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LimiteTempo: Limitacao {
    public float tempo;
    
    [System.NonSerialized]
    public Diretriz pai;

    class Listener: TimerListener {
        LimiteTempo limite;

        public Listener(LimiteTempo limite) {
            this.limite = limite;
        }

        public override void OnTimerEnd() {
            limite.Falhar();
        }
    }
    Listener listener;

    public LimiteTempo(float tempo) {
        this.tempo = tempo;
        listener = new Listener(this);
    }

    public void Iniciar() {
        TimerController.instance.AdicionarLimite(listener, tempo);
    }

    public void Interromper() {
        TimerController.instance.RemoverLimite(listener);
    }

    public void Falhar() {
        Debug.Log("Limite de tempo atingido");
        pai.Falhar();
    }
}

[CreateAssetMenu(fileName = "LimiteTempo", menuName = "Limitacoes/LimiteTempo")]
public class LimiteTempoObject: LimitacaoObject {
    public float tempo;

    public LimiteTempo Convert() {
        return new LimiteTempo(tempo);
    }
}