using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixas : MonoBehaviour
{
    bool isCarrying;
    public float speed = 5f;
    public Transform v;
    Rigidbody rb;
    Vector3 mover;
    Carga carga;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        v = GameObject.Find("Veiculo").transform;
        transform.rotation = v.rotation;
        isCarrying = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    private void Update()
    {
        float mZero;
        Debug.Log(Input.mouseScrollDelta.y);
        if (StartDrag.sd.currentState == StartDrag.State.Dirigindo)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        if(StartDrag.sd.SelectedObj == this.gameObject)
        {
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            mZero = Mathf.Clamp(Input.mouseScrollDelta.y, -1, 1);
            if (StartDrag.sd.currCam == StartDrag.sd.cams[2]) mover = new Vector3(-z, mZero, h);
            else if (StartDrag.sd.currCam == StartDrag.sd.cams[3]) mover = new Vector3(z, mZero, -h);
            else mover = new Vector3(h, mZero, z);
            Mover();
        }
    }
    void Mover()
    {
        Vector3 moveVector = transform.TransformDirection(mover) * speed;
        rb.velocity = moveVector;
    }
    public void Remover()
    {
        Destroy(this.gameObject);
    }
  private void OnMouseDown()
  {
        if (StartDrag.sd.currentState == StartDrag.State.Tetris)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.useGravity = false;
            StartDrag.sd.SelectedObj = this.gameObject;
        }
  }
}
