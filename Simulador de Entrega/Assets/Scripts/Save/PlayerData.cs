using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Vector3 position;
    public Vector3 rotation;
    public float dinheiro;
    public PlayerData (Vector3 p, Vector3 r, float d)
    {
        this.position = p;
        this.rotation = r;
        this.dinheiro = d;
    }
}
