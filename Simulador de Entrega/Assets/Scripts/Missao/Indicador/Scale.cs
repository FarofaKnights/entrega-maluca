using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour {
    public float speed = 1.0f;
    public Vector3 scale = Vector3.zero;
    Vector3 initialScale;

    [Range(0, 5)]
    public float variation = 0.1f;

    void Start() {
        initialScale = transform.localScale;
    }

    void Update() {
        float scaleVariation = Mathf.Sin(Time.time * speed) * variation;
        transform.localScale = initialScale + scale * scaleVariation;
    }
}
