using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixas : MonoBehaviour
{
    public bool rodando, dentroDoCarro = true, selecionado = false;
    public float multiplicadorDano;
    public Transform veiculo;
    public Caixas proxima, anterior;
    public GameObject Gizmos, spawnPosition;
    public AudioSource bater;
    Rigidbody rb;
    Vector3 posicaoInicial;
    Quaternion rotacaoInicial;
    public Carga carga;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Gizmos = Instantiate(Gizmos, transform.position, veiculo.transform.rotation);
        Gizmos.SetActive(false);
        posicaoInicial = transform.localPosition;
        rotacaoInicial = transform.localRotation;
        transform.rotation = veiculo.rotation;
        transform.SetParent(veiculo);
        spawnPosition = GameObject.Find("Veiculo/Cacamba/relocateCaixas");
        bater = GetComponent<AudioSource>();
    }
    public void ChecarLimites()
    {
        if (transform.localPosition.x > 7.2f && transform.localPosition.z > -6.5f) transform.localPosition = new Vector3(-7.2f, transform.localPosition.y, transform.localPosition.z);
        else if (transform.localPosition.x > 5f && transform.localPosition.z < -6.5f) transform.localPosition = new Vector3(-6f, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.x < -7.2f && transform.localPosition.z > -6.5f) transform.localPosition = new Vector3(7.2f, transform.localPosition.y, transform.localPosition.z);
        else if (transform.localPosition.x < -5f && transform.localPosition.z < -6.5f) transform.localPosition = new Vector3(6f, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.z < -7.5f) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 3f);
        if (transform.localPosition.z > 3f) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -7.5f);
    }
    public void Remover()
    {
        Destroy(this.gameObject);
    }
    void Selecionado()
    {
        transform.position = new Vector3(0, 3f, 0);
        rb.useGravity = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (Cacamba.instance.currentState == Cacamba.State.Dirigindo)
        {
            if (collision.gameObject.name != "Veiculo" && collision.gameObject.tag != gameObject.tag)
            {
                Debug.Log(carga.fragilidade);
                float velocity = rb.velocity.magnitude;
                carga.fragilidade -= velocity;
                carga.dentroCarro = false;
                bater.Play();
                if (carga.fragilidade <= 0)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    StartCoroutine(wait());
                }
            }
        }
        else
        {
            if (rb.velocity != Vector3.zero)
            {
               ChecarLimites();
            }
        }
    }
    void VoltarParaCarroca()
    {
        rb.constraints = RigidbodyConstraints.None;
        transform.position = spawnPosition.transform.position;
        dentroDoCarro = true;
        carga.dentroCarro = true;
    }
    private void FixedUpdate()
    {
        if (!dentroDoCarro)
        {
            if (Vector3.Distance(transform.position, spawnPosition.transform.position) <= 6)
            {
                Debug.Log(veiculo.GetComponent<Rigidbody>().velocity.magnitude);
                if (veiculo.GetComponent<Rigidbody>().velocity.magnitude <= 15)
                {
                    VoltarParaCarroca();
                }

            }
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
        dentroDoCarro = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void ResetarPosicao()
    {
        transform.rotation = rotacaoInicial;
        transform.position = posicaoInicial;
    }
}
