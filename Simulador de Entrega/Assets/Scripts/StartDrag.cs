using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDrag : MonoBehaviour
{
    public Camera [] cams;
    public Camera currCam;
    public Rigidbody rb;
    public Transform p1;
    public GameObject[] cargas;
    float f = 0;
    int u = 0;
    //rampa pra poder colocar os itens na caçamba; parede da parte de trás da caçamba, paredes invisiveis pra não arrastar os objetos pra fora da camera
    public GameObject parederetratil, player, paredeSide, rampa; 
    public static StartDrag sd;
    public bool completed = false, canRotate = false;
    int i = 0, load;
    void Start()
    {
        sd = this;
        parederetratil.SetActive(true);
        rb = Player.instance.GetComponent<Rigidbody>();
        currCam = cams[0];
        rampa.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            ChangeCam();
        }
    }
   // Entra no mode de colocar na caçamba
   public void changeCass()
    {
        //spawna a primeira caixa
        f = 0;
        u = 0;
        canRotate = false;
        cams[0].gameObject.SetActive(false);
        Debug.Log(Player.instance.cargaAtual.Count);
        cams[1].gameObject.SetActive(true);
        currCam = cams[1];
        UIController.instance.botaoConfirm.gameObject.SetActive(true);
        rb.isKinematic = true;
        rampa.SetActive(true);
        parederetratil.SetActive(false);
        //Spawna o resto das caixas com base na posição da caixa anterior
        foreach(Carga carga in Player.instance.cargaAtual)
        {
          GameObject caixa = Instantiate(cargas[Random.Range(0, cargas.Length)], new Vector3(p1.position.x, p1.position.y, p1.position.z + f), Quaternion.identity);
            carga.cx = caixa.GetComponent<Caixas>();
           f -= 0.2f;
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
       currCam = cams[0];
        rampa.SetActive(false);
        rb.isKinematic = false;
       parederetratil.SetActive(true);
       canRotate = true;
       paredeSide.GetComponent<MeshRenderer>().enabled = true;
    }
    void ChangeCam()
    {
        if(currCam == cams[1])
        {
            paredeSide.GetComponent<MeshRenderer>().enabled = false;
            cams[1].gameObject.SetActive(false);
            cams[2].gameObject.SetActive(true);
            currCam = cams[2];
        }
       else if (currCam == cams[2])
        {
            cams[2].gameObject.SetActive(false);
            cams[3].gameObject.SetActive(true);
            currCam = cams[3];
        }
        else if (currCam == cams[3])
        {
            paredeSide.GetComponent<MeshRenderer>().enabled = true;
            cams[3].gameObject.SetActive(false);
            cams[1].gameObject.SetActive(true);
            currCam = cams[1];
        }
    }
}
