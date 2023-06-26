using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixas : MonoBehaviour
{
    public bool rotating;
    public CaixasNoCarro cnoCarro;
    public float speed = 5f, rotateSpeed;
    public Transform v;
    public GameObject Gizmos;
    Rigidbody rb;
    Vector3 mover, rodar;
    public Carga carga;
    Transform refdrot;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        v = GameObject.Find("Veiculo").transform;
        refdrot = GameObject.Find("RefDRot").transform;
        refdrot.rotation = v.rotation;
        transform.rotation = v.rotation;
        Gizmos = Instantiate(Gizmos, transform.position, refdrot.transform.rotation);
        Gizmos.SetActive(false);
        transform.SetParent(v);
        cnoCarro = GetComponent<CaixasNoCarro>();
        cnoCarro.carga = carga;
        cnoCarro.carro = GameObject.Find("Veiculo");
        cnoCarro.rb = rb;
        cnoCarro.spawnPosition = GameObject.Find("Veiculo/Cacamba/relocateCaixas");
        cnoCarro.bater = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {   
        float mZero;
        if (StartDrag.sd.SelectedObj == this.gameObject)
        {
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            mZero = Mathf.Clamp(Input.mouseScrollDelta.y, -1, 1);
            rodar = new Vector3(mZero, h, z);
            if (StartDrag.sd.currCam == StartDrag.sd.cams[2]) mover = new Vector3(-z, mZero, h);
            else if (StartDrag.sd.currCam == StartDrag.sd.cams[3]) mover = new Vector3(z, mZero, -h);
            else mover = new Vector3(h, mZero, z);
            Mover();
        }
        else
        {
            rb.useGravity = true;
            Gizmos.SetActive(false);
            rotating = false;
        }
        if(rb.velocity != Vector3.zero)
        {
            Checar();
        }
    }
    private void Update()
    {
        if (StartDrag.sd.SelectedObj == this.gameObject)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (rotating)
                {
                    rotating = false;
                    Gizmos.SetActive(false);
                    rb.constraints = RigidbodyConstraints.None;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                }
                else
                {
                    rotating = true;
                    rb.velocity = Vector3.zero;
                    Gizmos.SetActive(true);
                    rb.constraints = RigidbodyConstraints.FreezePosition;
                }
            }
        }
    }
    void Mover()
    {
        if (!rotating)
        {
            Vector3 moveVector = v.TransformDirection(mover) * speed;
            rb.velocity = moveVector * Time.fixedDeltaTime;
            Gizmos.transform.position = transform.position;
        }
        else 
        {
            Vector3 rotateVector = refdrot.TransformDirection(rodar) * rotateSpeed;
            Quaternion delta = Quaternion.Euler(rotateVector * Time.fixedDeltaTime);
            rb.MoveRotation(delta * rb.rotation);
        }
    }
    void Checar()
    {
        if (transform.localPosition.x > 7.2f && transform.localPosition.z > -6.5f) transform.localPosition = new Vector3(-7.2f, transform.localPosition.y, transform.localPosition.z);
        else if (transform.localPosition.x > 5f && transform.localPosition.z < -6.5f) transform.localPosition = new Vector3(-6f, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.x < -7.2f && transform.localPosition.z > -6.5f) transform.localPosition = new Vector3(7.2f, transform.localPosition.y, transform.localPosition.z);
        else if (transform.localPosition.x < -5f && transform.localPosition.z < -6.5f) transform.localPosition = new Vector3(6f, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.z < -7.5f) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 3f);
        if (transform.localPosition.z > 3f) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -7.5f);
        if (transform.localPosition.y > 4f) StartDrag.sd.SelectedObj = null;
    }
    public void Remover()
    {
        Destroy(this.gameObject);
    }
    private void OnMouseDown()
    { 
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        StartDrag.sd.SelectedObj = this.gameObject;
  }
}
