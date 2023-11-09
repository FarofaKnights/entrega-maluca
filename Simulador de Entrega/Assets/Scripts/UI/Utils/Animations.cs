using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animations {
    public static IEnumerator Animation(float duration, Action<float> action) {
        float time = 0;
        while (time < duration) {
            action(time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        action(1);
    }

    public static IEnumerator UIScaleDown(GameObject go, float duration, Vector3 startScale, Vector3 endScale) {
        CanvasGroup canvasGroup = go.GetComponent<CanvasGroup>();
        
        Action<float> action = (float t) => {
            go.transform.localScale = Vector3.Lerp(startScale, endScale, t);
        };

        if (canvasGroup != null) {
            action = (float t) => {
                go.transform.localScale = Vector3.Lerp(startScale, endScale, t);
                canvasGroup.alpha = Mathf.Lerp(0, 1, t);
            };
        }

        return Animation(duration, action);
    }

    public static IEnumerator UIScaleDown(GameObject go, float duration, Vector3 startScale) {
        return UIScaleDown(go, duration, startScale, Vector3.one);
    }
}
