using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSubject : MonoBehaviour {
    public System.Action<Collision> onCollisionEnter;
    public System.Action<Collision> onCollisionExit;
    public System.Action<Collision> onCollisionStay;

    void OnCollisionEnter(Collision other) {
        if (onCollisionEnter != null) onCollisionEnter(other);
    }

    void OnCollisionExit(Collision other) {
        if (onCollisionExit != null) onCollisionExit(other);
    }

    void OnCollisionStay(Collision other) {
        if (onCollisionStay != null) onCollisionStay(other);
    }
}
