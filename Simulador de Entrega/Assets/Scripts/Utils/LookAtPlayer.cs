using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {
    void FixedUpdate() {
        transform.LookAt(Camera.main.transform);
    }
}
