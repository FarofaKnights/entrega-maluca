using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidrante : MonoBehaviour
{
    [SerializeField] float forca;
    void Action(Rigidbody rb)
    {
        rb.AddForce(transform.forward * forca, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Action(rb);
        }
    }
}
