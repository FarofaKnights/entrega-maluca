using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreiaMovedissa : MonoBehaviour
{
    public float divider;
    void Action(Rigidbody rb, Vector3 dir)
    {
        rb.AddForce(dir/divider, ForceMode.Acceleration);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //barulho.Play();
            //animacao.Play();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 dir = transform.position - other.transform.position;
            Action(rb, dir);
        }
    }
}
