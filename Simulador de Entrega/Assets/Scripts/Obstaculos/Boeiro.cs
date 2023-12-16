using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boeiro : MonoBehaviour
{
    Transform tampa;
    [SerializeField] AudioSource barulho;
    [SerializeField] float forca;
    Rigidbody rb;
    [SerializeField] ParticleSystem particle;
    void Start()
    {
        tampa = transform.GetChild(0);
        rb = tampa.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            rb.AddForce(Vector3.up * forca, ForceMode.Impulse);
            barulho.Play();
            particle.Play();
        }
    }
}
