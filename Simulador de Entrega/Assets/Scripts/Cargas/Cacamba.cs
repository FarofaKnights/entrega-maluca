using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cacamba : MonoBehaviour
{
    public enum State { Dirigindo, Tetris}
    public State currentState;
    public float speed, rotateSpeed;
    public GameObject [] cameras;
    public Rigidbody playerRb, caixaRb;
    public Transform[] pontos;
    public GameObject[] caixasNoCarro;
    public Caixas[] cargas, caixasCaidas;
    Vector3 rodar, mover;
    Transform veiculo;
    int cargaAtual = 0;
    public Caixas caixaAtual, primeira, ultima;
    public static Cacamba instance;
    public bool completed = false;
    [SerializeField] int i = 0, load, maxCaixas;
    private void Awake()
    {
        instance = this;
        currentState = State.Dirigindo;
    }
    void Start()
    {
        playerRb = Player.instance.GetComponent<Rigidbody>();
        veiculo = GameObject.Find("Veiculo").transform;
    }
    public void IniciarTetris()
    {
        cargaAtual = 0;
        maxCaixas = MissaoManager.instance.cargaAtual.Count;
        UIController.encaixe.Mostrar();
        playerRb.isKinematic = true;
        completed = false;
        i = 0;
        //Spawna o resto das caixas com base na posicao da caixa anterior
        foreach (Carga carga in MissaoManager.instance.cargaAtual)
        {
            GameObject c = Instantiate(carga.prefab, pontos[cargaAtual].position, carga.prefab.transform.rotation);
            cargas[cargaAtual] = c.GetComponent<Caixas>();
            c.GetComponent<Caixas>().veiculo = veiculo;
            c.GetComponent<Caixas>().carga = carga;
            c.GetComponent<Caixas>().carga.peso = c.GetComponent<Rigidbody>().mass;
            c.GetComponent<Caixas>().carga.fragilidade= c.GetComponent<Caixas>().fragilidade;
            c.GetComponent<Caixas>().carga._fragilidadeInicial = c.GetComponent<Caixas>().fragilidade;
            c.GetComponent<Caixas>().spawnPoint = pontos[cargaAtual];
            carga.cx = cargas[cargaAtual].GetComponent<Caixas>();
            cargaAtual++;
        }
        caixaAtual = cargas[0];
        caixaRb = caixaAtual.gameObject.GetComponent<Rigidbody>();
        caixaRb.constraints = RigidbodyConstraints.FreezeRotation;
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
        CriarListadeCaixas(cargas);
    }
    void CriarListadeCaixas(Caixas[] c)
    {
        int l;
        for(l = 0; c[l] != null; l++)
        {
            if(l != 0)
            {
                c[l].anterior = c[l - 1];
            }
            else
            {
                primeira = c[l];
            }
            if (c[l + 1] == null)
            {
                ultima = c[l];
            }
            else
            {
                c[l].proxima = c[l + 1];
            }
        }
        if(l - 1 <= 0)
        {
            ultima.anterior = ultima;
        }
        else
        {
            ultima.anterior = c[1 - 1];
        }
        primeira.anterior = ultima;
        ultima.proxima = primeira;
        currentState = State.Tetris;
        UIController.encaixe.SetCargas(cargas);
    }
    void Update()
    {
        if(currentState == State.Tetris)
        {
            //Ativa ou desativa o modo de rodar
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (caixaAtual.rodando)
                {
                   caixaAtual.rodando = false;
                   caixaAtual.Gizmos.SetActive(false);
                   caixaRb.constraints = RigidbodyConstraints.None;
                   caixaRb.constraints = RigidbodyConstraints.FreezeRotation;
                }
                else
                {
                    caixaAtual.rodando = true;
                    caixaRb.velocity = Vector3.zero;
                    caixaAtual.Gizmos.SetActive(true);
                    caixaRb.constraints = RigidbodyConstraints.FreezePosition;
                }
                //Trocar a caixaSelecionada
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                caixaAtual.rodando = false;
                caixaRb.useGravity = true;
                caixaAtual = caixaAtual.proxima;
                caixaRb = caixaAtual.gameObject.GetComponent<Rigidbody>();
                caixaRb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                caixaAtual.rodando = false;
                caixaRb.useGravity = true;
                caixaAtual = caixaAtual.anterior;
                caixaRb = caixaAtual.gameObject.GetComponent<Rigidbody>();
                caixaRb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                caixaAtual.rodando = false;
                caixaRb.useGravity = true;
                caixaAtual = caixaAtual.proxima.proxima;
                caixaRb = caixaAtual.gameObject.GetComponent<Rigidbody>();
                caixaRb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                caixaAtual.rodando = false;
                caixaRb.useGravity = true;
                caixaAtual = caixaAtual.anterior.anterior;
                caixaRb = caixaAtual.gameObject.GetComponent<Rigidbody>();
                caixaRb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            if(Input.GetKeyDown(KeyCode.X))
            {
                CaixaSelecionada();
            }
            if(Input.GetKeyDown(KeyCode.T))
            {
                caixaAtual.ResetarPosicao();
            }
          float h = Input.GetAxis("Horizontal");
          float v = Input.GetAxis("Vertical");
          rodar = new Vector3(v, h, 0);
          mover = new Vector3(h, 0, v);
          MoverCaixaSelecionada();
        }
    }
    void CaixaSelecionada()
    {
        if(caixaAtual.selecionado)
        {
            caixaRb.useGravity = true;
            caixaAtual.rodando = false;
            caixaAtual.Gizmos.SetActive(false);
            caixaRb.constraints = RigidbodyConstraints.None;
            caixaAtual.selecionado = false;
            caixaAtual = caixaAtual.proxima;
            caixaRb = caixaAtual.gameObject.GetComponent<Rigidbody>();
            caixaRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            caixaRb.useGravity = false;
            caixaRb.velocity = Vector3.zero;
            caixaAtual.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
            caixaAtual.Gizmos.transform.position = caixaAtual.transform.position;
            caixaAtual.selecionado = true;
        }
    }
    void MoverCaixaSelecionada()
    {
        if (!caixaAtual.rodando)
        {
            if (caixaAtual.selecionado)
            {
                Vector3 moveVector = caixaAtual.veiculo.TransformDirection(mover) * speed;
                caixaRb.velocity = moveVector * Time.fixedDeltaTime;
                caixaAtual.Gizmos.transform.position = caixaAtual.gameObject.transform.position;

            }
        }
        else
        {
            Vector3 rotateVector = caixaAtual.veiculo.TransformDirection(rodar) * rotateSpeed;
            Quaternion delta = Quaternion.Euler(rotateVector * Time.fixedDeltaTime);
            caixaRb.MoveRotation(delta * caixaRb.rotation);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(currentState == State.Tetris)
        {
            if(other.gameObject.CompareTag("Entrega") && other.isTrigger != true)
            {
               load = 0;
                while (caixasNoCarro[load] != null)
                    load++;
                caixasNoCarro[load] = other.gameObject;
                if (currentState == State.Tetris)
                {
                    i++;
                    if (i >= maxCaixas)
                    {
                        completed = true;
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(currentState == State.Tetris)
        {
            if (other.gameObject.CompareTag("Entrega") && other.isTrigger != true)
            {
                GameObject entrega = other.gameObject;
                for (int j = 0; j < caixasNoCarro.Length; j++)
                {
                    if (entrega == caixasNoCarro[j])
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
    }
    public void FinalizarTetris()
    {
        cameras[0].gameObject.SetActive(true);
        cameras[1].gameObject.SetActive(false);
        playerRb.isKinematic = false;
        UIController.encaixe.Esconder();
        currentState = State.Dirigindo;
        for(int m = 0; m < caixasCaidas.Length; m++)
        {
            if(caixasCaidas[m] != null)
            {
                caixasCaidas[m] = null;
            }
        }
    }
    public void MudarCaixas(Caixas [] c)
    {
        if(completed)
        {
            for (int r = 0; r < c.Length; r++)
            {
                if (c[r] != null)
                {
                    c[r].Gizmos.SetActive(false);
                    c[r].gameObject.transform.SetParent(null);
                    c[r].rb.constraints = RigidbodyConstraints.None;
                    c[r].rb.useGravity = true;
                }
            }
            FinalizarTetris();
        }
    }
    public void ReiniciarTetris()
    {
        UIController.encaixe.SetCargas(caixasCaidas);
        UIController.encaixe.Mostrar();
        completed = true;
        playerRb.isKinematic = true;
        CriarListadeCaixas(caixasCaidas);
        currentState = State.Tetris;
        for (int r = 0; r < caixasCaidas.Length; r++)
        {
            if(caixasCaidas[r] != null)
            {
                Caixas c = caixasCaidas[r];
                c.dentroDoCarro = true;
                c.gameObject.transform.SetParent(veiculo);
                c.Inicializar();
                c.spawnPoint = pontos[r];
                c.gameObject.transform.position = c.spawnPoint.position;
            }
        }
        caixaAtual = caixasCaidas[0];
        caixaRb = caixaAtual.gameObject.GetComponent<Rigidbody>();
        caixaRb.constraints = RigidbodyConstraints.FreezeRotation;
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);
    }
}
