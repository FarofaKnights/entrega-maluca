using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidrante : MonoBehaviour
{
    [SerializeField] float forca;
    [SerializeField] ParticleSystem particle;
    [SerializeField] AudioSource barulho;
    void Action(Rigidbody rb)
    {
        particle.Play();
        rb.AddForce(transform.forward * forca, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            barulho.Play();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Action(rb);
        }
    }
}
