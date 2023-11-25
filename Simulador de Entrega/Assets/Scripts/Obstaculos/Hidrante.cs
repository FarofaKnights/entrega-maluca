using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidrante : MonoBehaviour
{
    [SerializeField] float forca;
    public ParticleSystem particle;
    void Action(Rigidbody rb)
    {
        particle.Play();
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
