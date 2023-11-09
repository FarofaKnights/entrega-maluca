using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascaDeBanana : MonoBehaviour
{
    [SerializeField] float rotacao;
    AudioSource barulho;
    Animation animacao;
    void Action(Rigidbody rb)
    {
        rb.AddTorque(new Vector3(0, rotacao, 0), ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //barulho.Play();
            //animacao.Play();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Action(rb);
        }
    }
}
