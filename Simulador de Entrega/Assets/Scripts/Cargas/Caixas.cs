using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixas : MonoBehaviour
{
    public bool rodando, dentroDoCarro = true, selecionado = false;
    public float multiplicadorDano;
    public Transform veiculo, spawnPoint;
    public Caixas proxima, anterior;
    public GameObject Gizmos;
    public AudioSource bater;
    public Rigidbody rb;
    Vector3 posicaoInicial;
    Quaternion rotacaoInicial;
    public Carga carga;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Gizmos = Instantiate(Gizmos, transform.position, veiculo.transform.rotation);
        Gizmos.SetActive(false);
        bater = GetComponent<AudioSource>();
        Inicializar();
    }
    public void Inicializar()
    {
        transform.rotation = veiculo.rotation;
        rotacaoInicial = transform.rotation;
        transform.SetParent(veiculo);
    }
    public void ChecarLimites()
    {
        if(dentroDoCarro && transform.parent != null)
        { 
            if (transform.localPosition.x > 7.2f || transform.localPosition.x < -7.2f || transform.localPosition.z < -7.5f || transform.localPosition.z > 0.3f || transform.localPosition.y > 7.5f || transform.localPosition.y < -1f) ResetarPosicao();
        }
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
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!dentroDoCarro)
        {
            if(other.gameObject.name == "Veiculo")
            {
                for (int k = 0; k < Cacamba.instance.caixasCaidas.Length; k++)
                {
                    if (Cacamba.instance.caixasCaidas[k] == null)
                    {
                        Cacamba.instance.caixasCaidas[k] = this;
                        break;
                    }
                    else if(Cacamba.instance.caixasCaidas[k] == this)
                    {
                        break;
                    }
                    else
                    {
                        k++;
                    }
                }
                UIController.instance.botaoReiniciarTetris.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!dentroDoCarro)
        {
            if (other.gameObject.name == "Veiculo")
            {
                for (int k = 0; k < Cacamba.instance.caixasCaidas.Length; k++)
                {
                    if (Cacamba.instance.caixasCaidas[k] == this)
                    {
                        Cacamba.instance.caixasCaidas[k] = null;
                        break;
                    }
                    else
                    {
                        k++;
                    }
                }
                UIController.instance.botaoReiniciarTetris.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(!dentroDoCarro)
        {
            if(other.gameObject.name == "Veiculo")
            {
                UIController.instance.botaoReiniciarTetris.gameObject.SetActive(true);
            }
        }
    }
    private void FixedUpdate()
    {
        if (Cacamba.instance.currentState != Cacamba.State.Dirigindo)
        {
            ChecarLimites();
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
        rb.velocity = Vector3.zero;
        Gizmos.transform.position = transform.position;
        rodando = false;
        Gizmos.SetActive(false);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        transform.rotation = rotacaoInicial;
        if (!selecionado)
        {
            transform.position = spawnPoint.position;
        }
        else
        {
            transform.position = new Vector3(spawnPoint.position.x, transform.position.y, spawnPoint.position.z);
        }
    }
}
