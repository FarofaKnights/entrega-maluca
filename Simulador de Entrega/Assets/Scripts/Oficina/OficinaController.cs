using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OficinaController : MonoBehaviour {
    public static OficinaController instance;

    public GameObject veiculoHolder, sairRef;
    bool naOficina = false;

    public bool oficinaDisponivel = true;

    GameObject trigger;
    Camera cameraOficina;

    public List<IUpgrade> upgradesComprados = new List<IUpgrade>();

    // Solução temporária, talvez teremos que ver uma reestruturação geral do código (GameManager e Player)
    Dictionary<Renderer, Material[]> materiaisVeiculo = new Dictionary<Renderer, Material[]>();
    int defaultMaxSpeed;
    float defaultAcelleration;

    public void DesativarOficina() {
        oficinaDisponivel = false;
        trigger.SetActive(false);
    }

    public void AtivarOficina() {
        oficinaDisponivel = true;
        trigger.SetActive(true);
    }

    void Start() {
        instance = this;

        trigger = transform.Find("Trigger").gameObject;
        cameraOficina = transform.Find("Camera").GetComponent<Camera>();

        // Relativo aos Upgrades
        Renderer[] rends = Player.instance.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in rends) {
            materiaisVeiculo.Add(rend, rend.materials);
        }

        defaultMaxSpeed = Player.instance.GetComponent<WhellControler>().maxVelocidade;
        defaultAcelleration = Player.instance.GetComponent<WhellControler>().aceleracao;
    }

    public void EntrarOficina() {
        if (naOficina) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.SetParent(veiculoHolder.transform, false);
        player.GetComponent<Player>().enabled = false;
        player.GetComponent<WhellControler>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;

        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        player.transform.Find("Main Camera").gameObject.SetActive(false);
        cameraOficina.gameObject.SetActive(true);

        UIController.oficina.Mostrar();
        trigger.SetActive(false);

        naOficina = true;
    }

    public void SairOficina() {
        if (!naOficina) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.SetParent(null, true);
        player.GetComponent<Player>().enabled = true;
        player.GetComponent<WhellControler>().enabled = true;
        player.GetComponent<Rigidbody>().isKinematic = false;

        player.transform.position = sairRef.transform.position;
        player.transform.rotation = sairRef.transform.rotation;

        player.transform.Find("Main Camera").gameObject.SetActive(true);
        cameraOficina.gameObject.SetActive(false);
        
        UIController.oficina.Esconder();
        trigger.SetActive(true);

        naOficina = false;
    }

    #region Upgrades

    public void SetMaterial(Material material) {
        Renderer[] rends = Player.instance.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in rends) {
            if (material != null) rend.material = material;
            else rend.materials = materiaisVeiculo[rend];
        }
    }

    public void SetMotor(int maxSpeed, float acelleration) {
        if (maxSpeed < 0) maxSpeed = defaultMaxSpeed;
        if (acelleration < 0) acelleration = defaultAcelleration;

        Player.instance.GetComponent<WhellControler>().maxVelocidade = maxSpeed;
        Player.instance.GetComponent<WhellControler>().aceleracao = acelleration;
    }

    #endregion
}
