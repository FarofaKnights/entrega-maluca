using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUp : MonoBehaviour {
    public float speed = 1f;
    float _speed = 0f;
    public float delay = 0f;

    public float mult = 5f;


    bool moving = false;

    void Start() {
        _speed = speed;
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay() {
        yield return new WaitForSeconds(delay);
        moving = true;
    }

    void FixedUpdate() {
        if (!moving) return;

        transform.Translate(Vector3.up * _speed * Time.fixedDeltaTime);

        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform.anchoredPosition.y > rectTransform.rect.height) {
            rectTransform.anchoredPosition = new Vector2(0, rectTransform.rect.height - 25);
            moving = false;
        } else if (rectTransform.anchoredPosition.y < 0) {
            rectTransform.anchoredPosition = new Vector2(0, 0);
            moving = false;
        }
    }

    void Update() {
        if (Input.GetKey(KeyCode.S)) {
            _speed = speed * mult;
             moving = true;
        } else if (Input.GetKey(KeyCode.W)) {
            _speed = -speed * mult;
        }

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W)) {
            _speed = speed;
            moving = true;
        }
    }
}
