using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixas : MonoBehaviour
{
    bool isCarrying;
    public float speed = 5f, rotateSpeed;
    public Transform v;
    public GameObject Gizmos;
    Rigidbody rb;
    Vector3 mover, rodar;
    Carga carga;
    Transform filho, refdrot;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        v = GameObject.Find("Veiculo").transform;
        refdrot = GameObject.Find("RefDRot").transform;
        refdrot.rotation = v.rotation;
        filho = this.gameObject.transform.GetChild(0);
        transform.rotation = v.rotation;
        isCarrying = true;
        Gizmos = Instantiate(Gizmos, transform.position, refdrot.transform.rotation);
        Gizmos.SetActive(false);
    }
    private void Update()
    {
        float mZero;
        if (StartDrag.sd.SelectedObj == this.gameObject)
        {
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            if (Input.GetKey(KeyCode.Q)) rodar.x = 1;
            else rodar.x = 0;
            if (Input.GetKey(KeyCode.E)) rodar.y = 1;
            else rodar.y = 0;
            if (Input.GetKey(KeyCode.R)) rodar.z = 1;
            else rodar.z = 0;
            mZero = Mathf.Clamp(Input.mouseScrollDelta.y, -1, 1);
            if (StartDrag.sd.currCam == StartDrag.sd.cams[2]) mover = new Vector3(-z, mZero, h);
            else if (StartDrag.sd.currCam == StartDrag.sd.cams[3]) mover = new Vector3(z, mZero, -h);
            else mover = new Vector3(h, mZero, z);
            Mover();
        }
        else
        {
            rb.useGravity = true;
            this.gameObject.isStatic = true;
            Gizmos.SetActive(false);
        }
    }
    void Mover()
    {
        Vector3 moveVector = v.TransformDirection(mover) * speed;
        rb.velocity = moveVector * Time.deltaTime;
        Vector3 rotateVector = refdrot.TransformDirection(rodar) * rotateSpeed;
        Quaternion delta = Quaternion.Euler(rotateVector * Time.deltaTime);
        rb.MoveRotation(delta * rb.rotation);
        Gizmos.transform.position = transform.position;
    }
    public void Remover()
    {
        Destroy(this.gameObject);
    }
  private void OnMouseDown()
  {
        if (StartDrag.sd.currentState == StartDrag.State.Tetris)
        {
            this.gameObject.isStatic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.useGravity = false;
            StartDrag.sd.SelectedObj = this.gameObject;
            Gizmos.SetActive(true);
        }
  }
}
