using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDrag : MonoBehaviour
{
    public Camera principal, cassamba;
    public Rigidbody rb;
    public Transform p1;
    public GameObject[] cargas;
    public GameObject player;
    float f = 0;
    int u = 0;
    //rampa pra poder colocar os itens na caçamba; parede da parte de trás da caçamba, paredes invisiveis pra não arrastar os objetos pra fora da camera
    public GameObject rampa, parederetratil, pinvis; 
    public static StartDrag sd;
    public bool completed = false, makechild = false;
    int i = 0, load;
    void Start()
    {
        sd = this;
        rampa.SetActive(false);
        parederetratil.SetActive(true);
        pinvis.SetActive(false);
        rb = Player.instance.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   // Entra no mode de colocar na caçamba
   public void changeCass()
    {
        //spawna a primeira caixa
        f = 0;
        u = 0;
        makechild = false;
        principal.gameObject.SetActive(false);
        Debug.Log(Player.instance.cargaAtual.Count);
        cassamba.gameObject.SetActive(true);
        UIController.instance.botaoConfirm.gameObject.SetActive(true);
        pinvis.SetActive(true);
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
       principal.gameObject.SetActive(true);
       cassamba.gameObject.SetActive(false);
       pinvis.SetActive(false);
       rb.isKinematic = false;
       rampa.SetActive(false);
       parederetratil.SetActive(true);
       makechild = true;
    }
}
