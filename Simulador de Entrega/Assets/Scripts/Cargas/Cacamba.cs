using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cacamba : MonoBehaviour
{
    public enum State { Dirigindo, Tetris}
    public State currentState;
    public GameObject [] cameras;
    public GameObject cameraAtual;
    public Rigidbody rb;
    public Transform[] pontos;
    public GameObject[] cargas, caixasNoCarro;
    int u = 0;
    public GameObject objSelecionado; 
    public static Cacamba instance;
    public bool completed = false;
    [SerializeField] int i = 0, load;
    private void Awake()
    {
        instance = this;
        currentState = State.Dirigindo;
    }
    void Start()
    {
        rb = Player.instance.GetComponent<Rigidbody>();
        cameraAtual = cameras[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(currentState == State.Tetris)
            {

              cameras[1].gameObject.SetActive(true);
              cameras[2].gameObject.SetActive(false);
              cameras[3].gameObject.SetActive(false);
              cameras[4].gameObject.SetActive(false);
              cameraAtual = cameras[1];
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (currentState == State.Tetris)
            {
                cameras[1].gameObject.SetActive(false);
                cameras[2].gameObject.SetActive(true);
                cameras[3].gameObject.SetActive(false);
                cameras[4].gameObject.SetActive(false);
                cameraAtual = cameras[2];
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentState == State.Tetris)
            {
                cameras[1].gameObject.SetActive(false);
                cameras[2].gameObject.SetActive(false);
                cameras[3].gameObject.SetActive(true);
                cameras[4].gameObject.SetActive(false);
                cameraAtual = cameras[3];
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentState == State.Tetris)
            {

                cameras[1].gameObject.SetActive(false);
                cameras[2].gameObject.SetActive(false);
                cameras[3].gameObject.SetActive(false);
                cameras[4].gameObject.SetActive(true);
                cameraAtual = cameras[4];
            }
        }
    }
   public void IniciarTetris()
    {
        currentState = State.Tetris;
        u = 0;
        i = 0;
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
        cameraAtual = cameras[1];
        UIController.encaixe.Mostrar();
        rb.isKinematic = true;
        //Spawna o resto das caixas com base na posicao da caixa anterior
        foreach (Carga carga in MissaoManager.instance.cargaAtual)
        {
           GameObject caixa = Instantiate(carga.prefab, pontos[u].position, carga.prefab.transform.rotation);
           carga.cx = caixa.GetComponent<Caixas>();
           u++;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Entrega"))
        {
           load = 0;
            while (caixasNoCarro[load] != null)
                load++;
            caixasNoCarro[load] = other.gameObject;
            if (currentState == State.Tetris)
            {
                i++;
                if (i >= MissaoManager.instance.cargaAtual.Count)
                {
                    completed = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Entrega"))
        {
            GameObject ligma = other.gameObject;
            CaixasNoCarro cnc = ligma.GetComponent<CaixasNoCarro>();
            for (int j = 0; j < caixasNoCarro.Length; j++)
            {
                if (ligma == caixasNoCarro[j])
                { 
                    caixasNoCarro[j] = null;
                }
            }
            if (currentState == State.Tetris)
            {
                i -= 1;
                completed = false;
            }
        }
    }
    public void FinalizarTetris()
    {
        if (completed)
        {
            cameras[0].gameObject.SetActive(true);
            cameras[1].gameObject.SetActive(false);
            cameras[2].gameObject.SetActive(false);
            cameras[3].gameObject.SetActive(false);
            cameras[4].gameObject.SetActive(false);
            cameraAtual = cameras[0];
            rb.isKinematic = false;
            currentState = State.Dirigindo;
            objSelecionado = null;
            MudarCaixas();
        }
    }

    void MudarCaixas()
    {
        for (int h = 0; h < caixasNoCarro.Length; h++)
        {
            if(caixasNoCarro[h] == null)
            {
               break;
            }
            Rigidbody rb = caixasNoCarro[h].GetComponent<Rigidbody>();
            caixasNoCarro[h].transform.SetParent(null);
            Caixas c = caixasNoCarro[h].GetComponent<Caixas>();
            c.Gizmos.SetActive(false);
            c.caixasnoCarro.enabled = true;
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            c.enabled = false;
        }
    }
}
