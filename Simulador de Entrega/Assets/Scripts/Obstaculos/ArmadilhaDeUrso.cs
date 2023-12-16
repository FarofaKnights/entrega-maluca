using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilhaDeUrso : MonoBehaviour
{
    [SerializeField] AudioSource barulho;
    Animation animacao;
    [SerializeField] float tempo;
    void Action(Rigidbody rb)
    {
        rb.velocity -= rb.velocity;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(Wait(rb));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            barulho.Play();
            //animacao.Play();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Action(rb);
        }
    }
    IEnumerator Wait(Rigidbody rb)
    {
        yield return new WaitForSeconds(tempo);
        rb.constraints = RigidbodyConstraints.None;
    }
}
