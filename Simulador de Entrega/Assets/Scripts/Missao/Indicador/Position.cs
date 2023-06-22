using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour {
    public float speed = 1.0f;
    public Vector3 direction = Vector3.zero;
    Vector3 initialPosition;

    [Range(0, 5)]
    public float variation = 0.1f;

    void Start() {
        initialPosition = transform.position;
    }

    void Update() {
        float pos = Mathf.Sin(Time.time * speed) * variation + 1;

        transform.position = initialPosition + direction * pos;
    }
}
