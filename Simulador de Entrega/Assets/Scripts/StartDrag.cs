using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDrag : MonoBehaviour
{
    public Camera [] cams;
    public Camera currCam;
    public Rigidbody rb;
    public Transform[] pontos;
    public GameObject[] cargas;
    int u = 0;
    //rampa pra poder colocar os itens na caçamba; parede da parte de trás da caçamba, paredes invisiveis pra não arrastar os objetos pra fora da camera
    public GameObject parederetratil, player, paredeSide; 
    public static StartDrag sd;
    public bool completed = false, canRotate = false;
    int i = 0, load;
    void Start()
    {
        sd = this;
        parederetratil.SetActive(true);
        rb = Player.instance.GetComponent<Rigidbody>();
        currCam = cams[0];
    }

    // Update is called once per frame
    void Update()
    {
            ChangeCam();
    }
   // Entra no mode de colocar na caçamba
   public void changeCass()
    {
        u = 0;
        canRotate = false;
        cams[0].gameObject.SetActive(false);
        Debug.Log(Player.instance.cargaAtual.Count);
        cams[1].gameObject.SetActive(true);
        currCam = cams[1];
        UIController.instance.botaoConfirm.gameObject.SetActive(true);
        rb.isKinematic = true;
        parederetratil.SetActive(false);
        //Spawna o resto das caixas com base na posição da caixa anterior
        foreach(Carga carga in Player.instance.cargaAtual)
        {
           int p = Random.Range(0, cargas.Length);
           GameObject caixa = Instantiate(cargas[p], pontos[u].position, cargas[p].transform.rotation);
           carga.cx = caixa.GetComponent<Caixas>();
           u++;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Entrega"))
        {
            i += 1;
            Debug.Log(i);
            if(i >= Player.instance.cargaAtual.Count)
            {
                completed = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Entrega"))
        {
            i -= 1;
            Debug.Log(i);
            completed = false;
        }
    }
    public void Confirm()
    {
       cams[0].gameObject.SetActive(true);
       cams[1].gameObject.SetActive(false);
       cams[2].gameObject.SetActive(false);
       cams[3].gameObject.SetActive(false);
       cams[4].gameObject.SetActive(false);
       currCam = cams[0];
       rb.isKinematic = false;
       parederetratil.SetActive(true);
       canRotate = true;
       paredeSide.GetComponent<MeshRenderer>().enabled = true;
    }
    void ChangeCam()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            cams[1].gameObject.SetActive(true);
            cams[2].gameObject.SetActive(false);
            cams[3].gameObject.SetActive(false);
            cams[4].gameObject.SetActive(false);
            currCam = cams[1];
            paredeSide.GetComponent<MeshRenderer>().enabled = true;
        }
       else if (Input.GetKeyDown(KeyCode.H))
        {
            cams[1].gameObject.SetActive(false);
            cams[2].gameObject.SetActive(true);
            cams[3].gameObject.SetActive(false);
            cams[4].gameObject.SetActive(false);
            currCam = cams[2];
            paredeSide.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            cams[1].gameObject.SetActive(false);
            cams[2].gameObject.SetActive(false);
            cams[3].gameObject.SetActive(true);
            cams[4].gameObject.SetActive(false);
            currCam = cams[3];
            paredeSide.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            cams[1].gameObject.SetActive(false);
            cams[2].gameObject.SetActive(false);
            cams[3].gameObject.SetActive(false);
            cams[4].gameObject.SetActive(true);
            currCam = cams[4];
            paredeSide.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
