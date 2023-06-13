using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixasNoCarro : MonoBehaviour
{
   public Rigidbody rb;
   public Carga carga;
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
            }
        }
    }
}
