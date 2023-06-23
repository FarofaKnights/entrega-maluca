using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixasNoCarro : MonoBehaviour
{
    public Rigidbody rb;
    public Carga carga;
    public GameObject spawnPosition, carro;
    public bool inCarro = true;
    public float multiplicadorDano;
    private void OnCollisionEnter(Collision collision)
    {
        if (StartDrag.sd.currentState == StartDrag.State.Dirigindo)
        {
            if (collision.gameObject.name != "Veiculo" && collision.gameObject.tag != gameObject.tag)
            {
                Debug.Log(carga.fragilidade);
                float velocity = rb.velocity.magnitude;
                carga.fragilidade -= velocity;
                carga.dentroCarro = false;
                StartCoroutine(wait());
                if(carga.fragilidade <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    void ChangePos()
    {
        rb.constraints = RigidbodyConstraints.None;
        transform.position = spawnPosition.transform.position;
        inCarro = true;
        carga.dentroCarro = true;
    }
    private void Update()
    {
        if (!inCarro)
        {
            if (Vector3.Distance(transform.position, spawnPosition.transform.position) <= 6)
            {
                Debug.Log(carro.GetComponent<Rigidbody>().velocity.magnitude);
                if (carro.GetComponent<Rigidbody>().velocity.magnitude <= 15)
                {
                    ChangePos();
                }

            }
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
        inCarro = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
