using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilhaDeUrso : MonoBehaviour
{
    [SerializeField] AudioSource barulho;
    [SerializeField] float tempo;
    public Animator anim;
    public bool hasAnim = true;

    void Action(Rigidbody rb)
    {
        rb.velocity -= rb.velocity;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        barulho.Play();
        if(hasAnim)
        {
            anim.SetBool("Desativo", false);
            anim.SetBool("Ativo", true);
        }
        StartCoroutine(Wait(rb));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Action(rb);
        }
    }
    IEnumerator Wait(Rigidbody rb)
    {
        yield return new WaitForSeconds(tempo);
        if(hasAnim)
        {
            anim.SetBool("Desativo", true);
            anim.SetBool("Ativo", false);
        }
        rb.constraints = RigidbodyConstraints.None;
        if(hasAnim)
        {
             anim.SetBool("Desativo", false);
        }
    }
}
