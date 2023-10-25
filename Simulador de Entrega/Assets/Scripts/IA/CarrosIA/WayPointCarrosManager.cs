using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointCarrosManager : MonoBehaviour
{
    public Transform[] wayPoints;
    public Transform[] stopPoints;
    public static WayPointCarrosManager instance;
    void Awake()
    {
        instance = this;  
    }
}
