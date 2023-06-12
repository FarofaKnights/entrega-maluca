using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour {
    public static MinimapManager instance;

    public Camera minimapCamera;
    public GameObject iconPrefab;
    public float minimapHeight = 48;

    void Awake () {
        instance = this;
    }
}
