using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNPCFactory : MonoBehaviour {
    public GameObject[] prefabs;

    public GameObject GetRandomPrefab() {
        return prefabs[Random.Range(0, prefabs.Length)];
    }
}
