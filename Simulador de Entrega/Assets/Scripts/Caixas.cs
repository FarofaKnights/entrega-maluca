using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixas : MonoBehaviour
{
    public static Caixas cx;
    bool selcted = false;
    public float spinspeed = 5f;
    GameObject vaiculo;
    Transform v;
    Rigidbody rb;
    Vector3 angularVelocity;
    Carga carga;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cx = this;
        rb.mass = Random.Range(0, 21);
        vaiculo = GameObject.Find("Veiculo");
        v = vaiculo.transform;
    }
    private void Update()
    {
        if(StartDrag.sd.makechild)
        {
            gameObject.transform.parent = v;
            Destroy(rb);
        }
    }
    public void Remover()
    {
        Destroy(this.gameObject);
    }

    //MEIO BUGADO AINDA!!!

    /*   private void OnMouseDown()
       {
           selcted = true;
       }
       private void OnMouseUp()
       {
           selcted = false;
       }
       void Update()
       {
           float h = Input.GetAxis("Horizontal") * spinspeed;
           float v = Input.GetAxis("Vertical") * spinspeed;
           angularVelocity = new Vector3(v, h, 0f);
           Quaternion deltaRotation = Quaternion.Euler(angularVelocity * Time.fixedDeltaTime);
           if (selcted)
           {
               rb.MoveRotation(rb.rotation * deltaRotation);
           }
       }*/
}
