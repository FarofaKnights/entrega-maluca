using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour {
    public Transform target;
    Camera minimapCamera;

    float cameraSize, cameraWidth, cameraHeight;
    float iconWidth, iconHeight;
    float radius;

    void Start() {
        minimapCamera = MinimapManager.instance.minimapCamera;

        cameraSize = minimapCamera.orthographicSize;
        cameraWidth = cameraSize * 2 * minimapCamera.aspect;
        cameraHeight = cameraSize * 2;

        iconWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        iconHeight = GetComponent<SpriteRenderer>().bounds.size.y;

        radius = Mathf.Max(cameraWidth, cameraHeight) / 2 - iconWidth / 2;
    }
    
    void Update() {
        Vector3 pos = target.position;
        pos.y = transform.position.y;
        transform.position = pos;

        // Check if icon is inside camera view
        Vector3 center = minimapCamera.transform.position;
        center.y = transform.position.y;

        Vector3 directionToIcon = transform.position - center;

        if (directionToIcon.magnitude > radius) {
            // Icon is outside camera view
            // Clamp icon position to circle
            directionToIcon = directionToIcon.normalized * radius;

            Vector3 clampedPosition = center + directionToIcon;
            clampedPosition.y = transform.position.y;
            transform.position = clampedPosition;
        }
    }
}
