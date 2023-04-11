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
        float mZero;
        if (StartDrag.sd.canRotate)
        {
            rb.constraints = RigidbodyConstraints.None;
            gameObject.transform.SetParent(null);
            isCarrying = false;
        }
        if(selcted)
        {
            Vector3 mover;
            float h = Input.GetAxis("Horizontal") * speed;
            float z = Input.GetAxis("Vertical") * speed;
            if (transform.localPosition.y >= 0f && transform.localPosition.y <= 0.3f) mZero = Input.mouseScrollDelta.y;
            else mZero = 0f;
            if (StartDrag.sd.currCam == StartDrag.sd.cams[2]) mover = new Vector3(-z, mZero * speed, h);
            else if (StartDrag.sd.currCam == StartDrag.sd.cams[3]) mover = new Vector3(z, mZero * speed, -h);
            else mover = new Vector3(h, mZero * speed, z);
            transform.localPosition += (mover * Time.deltaTime);

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
