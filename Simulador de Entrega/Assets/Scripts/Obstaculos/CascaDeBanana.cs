using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascaDeBanana : MonoBehaviour
{
    [SerializeField] float rotacao;
    [SerializeField] AudioSource barulho;
    void Action(Rigidbody rb)
    {
        rb.AddTorque(new Vector3(0, rotacao, 0), ForceMode.Impulse);
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
