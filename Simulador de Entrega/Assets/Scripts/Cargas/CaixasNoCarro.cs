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
                inCarro = false;if(carga.fragilidade <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    void ChangePos()
    {
        transform.position = spawnPosition.transform.position + carro.GetComponent<Rigidbody>().velocity/2;
        inCarro = true;
    }
    private void Update()
    {
        if (!inCarro)
        {
            if (Vector3.Distance(transform.position, carro.transform.position) <= 5)
            {
                ChangePos();
            }
        }
    }
}
