using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimerObject {
    public float tempo;
    public LimiteTempo Convert() {
        return new LimiteTempo(tempo);
    }

}
