using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixas : MonoBehaviour
{
    public static Caixas cx;
    bool selcted = false, isCarrying;
    public float speed = 5f;
    public Transform v;
    Rigidbody rb;
    public Vector3 angularVelocity;
    Carga carga;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cx = this;
        rb.mass = Random.Range(0, 21);
        v = GameObject.Find("Veiculo").transform;
        gameObject.transform.SetParent(v);
        isCarrying = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    private void Update()
    {
        if (StartDrag.sd.canRotate)
        {
            rb.constraints = RigidbodyConstraints.None;
            gameObject.transform.SetParent(null);
            isCarrying = false;
        }
        if(selcted)
        {
            float h = Input.GetAxis("Horizontal") * speed;
            float z = Input.GetAxis("Vertical") * speed;
            angularVelocity = new Vector3(h, 0, z);
            Quaternion deltaRotation = Quaternion.Euler(angularVelocity * Time.fixedDeltaTime);
            rb.MoveRotation(transform.rotation * deltaRotation);
        }
        if (isCarrying)
        {
            if (transform.localPosition.x >= 0.56f) rb.MovePosition(new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z));
            if (transform.localPosition.x <= -0.56f) rb.MovePosition(new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z));
            if (transform.localPosition.z >= -0.122f) rb.MovePosition(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f));
            if (transform.localPosition.z <= -0.78f) rb.MovePosition(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f));
        }
    }
    public void Remover()
    {
        Destroy(this.gameObject);
    }
  private void OnMouseDown()
  {
    selcted = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
  }
  private void OnMouseUp()
  {
    selcted = false;
    rb.useGravity = true;
    rb.constraints = RigidbodyConstraints.None;
  }
}
