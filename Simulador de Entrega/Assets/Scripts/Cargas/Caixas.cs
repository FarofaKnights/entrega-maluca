using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixas : MonoBehaviour
{
    bool rotating;
    public CaixasNoCarro cnoCarro;
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
        Gizmos = Instantiate(Gizmos, transform.position, refdrot.transform.rotation);
        Gizmos.SetActive(false);
        cnoCarro = GetComponent<CaixasNoCarro>();
    }
    private void Update()
    {
        float mZero;
        if (StartDrag.sd.SelectedObj == this.gameObject)
        {
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            mZero = Input.mouseScrollDelta.y;
            rodar = new Vector3(mZero, h, z);
            if (StartDrag.sd.currCam == StartDrag.sd.cams[2]) mover = new Vector3(-z, mZero, h);
            else if (StartDrag.sd.currCam == StartDrag.sd.cams[3]) mover = new Vector3(z, mZero, -h);
            else mover = new Vector3(h, mZero, z);
            Mover();
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (rotating)
                {
                    rotating = false;
                    Gizmos.SetActive(false);
                }
                else
                {
                    rotating = true;
                    Gizmos.SetActive(true);
                }
            }
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
        if (!rotating)
        {
            Vector3 moveVector = v.TransformDirection(mover) * speed;
            rb.velocity = moveVector * Time.deltaTime;
            Gizmos.transform.position = transform.position;
        }
        else 
        {
            Vector3 rotateVector = refdrot.TransformDirection(rodar) * rotateSpeed;
            Quaternion delta = Quaternion.Euler(rotateVector * Time.deltaTime);
            rb.MoveRotation(delta * rb.rotation);
        }
    }
    public void Remover()
    {
        Destroy(this.gameObject);
    }
    private void OnMouseDown()
    { 
        this.gameObject.isStatic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        StartDrag.sd.SelectedObj = this.gameObject;
  }
}
