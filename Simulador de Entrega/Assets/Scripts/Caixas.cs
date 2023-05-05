using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixas : MonoBehaviour
{
    public static Caixas cx;
    bool selcted = false, isCarrying;
    public float speed = 5f, scrollSpeed;
    public Transform v;
    Rigidbody rb;
    public Vector3 angularVelocity;
    Carga carga;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cx = this;
        rb.mass = 1;
        v = GameObject.Find("Veiculo").transform;
        gameObject.transform.SetParent(v);
        isCarrying = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    private void Update()
    {
        float mZero;
        Debug.Log(Input.mouseScrollDelta.y);
        if (StartDrag.sd.canRotate)
        {
            rb.constraints = RigidbodyConstraints.None;
            gameObject.transform.SetParent(null);
            isCarrying = false;
        }
        if(selcted && isCarrying)
        {
            Vector3 mover;
            float h = Input.GetAxis("Horizontal") * speed;
            float z = Input.GetAxis("Vertical") * speed;
            mZero = Mathf.Clamp(Input.mouseScrollDelta.y, -1, 1);
            mZero *= speed;
            if (StartDrag.sd.currCam == StartDrag.sd.cams[2]) mover = new Vector3(-z, mZero, h);
            else if (StartDrag.sd.currCam == StartDrag.sd.cams[3]) mover = new Vector3(z, mZero, -h);
            else mover = new Vector3(h, mZero, z);
            //rb.AddForceAtPosition(mover, transform.position);
            /*if (transform.localPosition.x <= 0.52f && Input.GetKeyDown(KeyCode.D)) rb.MovePosition(new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z));
            if (transform.localPosition.x >= -0.52f && Input.GetKeyDown(KeyCode.A)) rb.MovePosition(new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z));
            if (transform.localPosition.z <= -0.122f && Input.GetKeyDown(KeyCode.W)) rb.MovePosition(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f));
            if (transform.localPosition.z >= -0.78f && Input.GetKeyDown(KeyCode.S)) rb.MovePosition(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f));
            */
            rb.MovePosition(transform.position + mover * Time.deltaTime);
        }
        if (isCarrying)
        {
            /*if (transform.localPosition.x >= 2.62f) rb.MovePosition(new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z));
            if (transform.localPosition.x <= -2.62f) rb.MovePosition(new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z));
            if (transform.localPosition.z >= -3f) rb.MovePosition(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f));
            if (transform.localPosition.z <= -3f) rb.MovePosition(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f));
            if (transform.localPosition.y >= 2.3f) rb.MovePosition(new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z));
            */
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
    rb.constraints &= ~RigidbodyConstraints.FreezeRotation;
  }
}
