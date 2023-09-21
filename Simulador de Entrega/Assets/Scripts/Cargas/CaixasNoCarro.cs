using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixasNoCarro : MonoBehaviour
{
    public Rigidbody rb;
    public Carga carga;
    public GameObject spawnPosition, carro;
    public bool dentroDoCarro = true;
    public float multiplicadorDano;
   public AudioSource bater;
    private void OnCollisionEnter(Collision collision)
    {
        if (Cacamba.instance.currentState == Cacamba.State.Dirigindo)
        {
            if (collision.gameObject.name != "Veiculo" && collision.gameObject.tag != gameObject.tag)
            {
                Debug.Log(carga.fragilidade);
                float velocity = rb.velocity.magnitude;
                carga.fragilidade -= velocity;
                carga.dentroCarro = false;
                bater.Play();
                if(carga.fragilidade <= 0)
                {
                    gameObject.SetActive(false);
                } else 
                {
                    StartCoroutine(wait());
                }
            }
        }
    }
    void VoltarParaCarroca()
    {
        rb.constraints = RigidbodyConstraints.None;
        transform.position = spawnPosition.transform.position;
        dentroDoCarro = true;
        carga.dentroCarro = true;
    }
    private void FixedUpdate()
    {
        if (!dentroDoCarro)
        {
            if (Vector3.Distance(transform.position, spawnPosition.transform.position) <= 6)
            {
                Debug.Log(carro.GetComponent<Rigidbody>().velocity.magnitude);
                if (carro.GetComponent<Rigidbody>().velocity.magnitude <= 15)
                {
                    VoltarParaCarroca();
                }

            }
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
        dentroDoCarro = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}