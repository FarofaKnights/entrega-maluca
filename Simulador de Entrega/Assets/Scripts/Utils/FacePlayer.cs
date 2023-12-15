using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour {
    public bool x = true;
    public bool y = true;
    public bool z = true;

    public Transform secundaryTarget;


    void FixedUpdate() {
        Transform player = (secundaryTarget==null) ? Player.instance.transform : secundaryTarget;

        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        if (x) {
            direction.x = 0;
        }

        if (y) {
            direction.y = 0;
        }

        if (z) {
            direction.z = 0;
        }

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
