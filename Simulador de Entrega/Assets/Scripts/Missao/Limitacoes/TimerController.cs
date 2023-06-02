using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerListener {
    public virtual void OnTimerStart() {}
    public virtual void OnTimerEnd() {}
    public virtual void OnTimerUpdate(float tempo) {}
}

public class TimerController : MonoBehaviour {
    public static TimerController instance;

    public Dictionary<TimerListener, float> limites = new Dictionary<TimerListener, float>();

    void Start() {
        instance = this;
    }

    void Update() {
        foreach (TimerListener listener in limites.Keys) {
            limites[listener] -= Time.deltaTime;
            listener.OnTimerUpdate(limites[listener]);

            if (limites[listener] <= 0) {
                listener.OnTimerEnd();
                limites.Remove(listener);
            }
        }
    }

    public void AdicionarLimite(TimerListener listener, float tempo) {
        limites.Add(listener, tempo);
        listener.OnTimerStart();
    }

    public void RemoverLimite(TimerListener listener) {
        limites.Remove(listener);
        listener.OnTimerEnd();
    }
}
