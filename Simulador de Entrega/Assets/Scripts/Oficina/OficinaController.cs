using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OficinaController : MonoBehaviour {
    public static OficinaController instance;

    Controls controls;
    float girarValue = 0f;
    bool girando = false;
    public float giroPadrao = 1f;

    public GameObject veiculoHolder, sairRef, giradorRef;
    bool naOficina = false;

    public bool oficinaDisponivel = true;

    GameObject trigger;
    Camera cameraOficina;

    // Pelo amor de deus não muda isso pra public, sério
    [SerializeField] List<UpgradeObject> upgradesAtivos = new List<UpgradeObject>();
    public List<UpgradeObject> upgradesComprados = new List<UpgradeObject>();
    public List<UpgradeObject> upgrades = new List<UpgradeObject>();
    

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

    public UpgradeData GetUpgradeData()
    {
        string[] comprados = new string[upgradesComprados.Count];
        string[] ativos = new string[upgradesAtivos.Count];

        int i = 0;
        foreach (UpgradeObject up in upgradesComprados)
        {
            comprados[i] = up.name;
            i++;
        }
        UpgradeData ud = new UpgradeData(comprados);
        return ud;
    }

    public void SetUpgradeData(UpgradeData up)
    {
        foreach (string names in up.compradosNomes)
        {
            for(int i = 0; i < upgrades.Count; i++)
            {
                if(upgrades[i].name == names)
                {
                    upgradesComprados.Add(upgrades[i]);

                    int indexOf = upgrades.IndexOf(upgrades[i]);
                    if (indexOf != -1) {
                        AtivarUpgrade(upgrades[i]);
                    }
                }
            }
        }
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

        player.GetComponent<Player>().cameras[0].gameObject.SetActive(false);
        cameraOficina.gameObject.SetActive(true);

        UIController.oficina.Mostrar();
        trigger.SetActive(false);

        controls = new Controls();
        controls.Oficina.Girar.performed += ctx => HabilitarGirar(ctx.ReadValue<float>());
        controls.Oficina.Girar.canceled += ctx => DesabilitarGirar();
        controls.Oficina.Enable();

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

        player.GetComponent<Player>().cameras[0].gameObject.SetActive(true);
        cameraOficina.gameObject.SetActive(false);
        
        UIController.oficina.Esconder();
        trigger.SetActive(true);

        controls.Oficina.Disable();

        naOficina = false;
    }

    #region Upgrades

    public void SetMotor(int maxSpeed, float acelleration) {
        if (maxSpeed < 0) maxSpeed = defaultMaxSpeed;
        if (acelleration < 0) acelleration = defaultAcelleration;

        Player.instance.GetComponent<WhellControler>().maxVelocidade = maxSpeed;
        Player.instance.GetComponent<WhellControler>().aceleracao = acelleration;
    }

    public void AtivarUpgrade(UpgradeObject upgrade) {
        if (upgradesAtivos.Contains(upgrade)) return;

        if (upgrade.exclusivo) {
            // Evita multiplos por quaisquer motivos
            List<UpgradeObject> exclusivos = new List<UpgradeObject>();
            
            foreach (UpgradeObject up in upgradesAtivos) {
                if (upgrade.CalculoDeExclusividade(up)) {
                    exclusivos.Add(up);
                }
            }

            foreach (UpgradeObject up in exclusivos) {
                DesativarUpgrade(up);
            }
        }

        upgradesAtivos.Add(upgrade);

        upgrade.Ativar();
    }

    public void DesativarUpgrade(UpgradeObject upgrade) {
        if (!upgradesAtivos.Contains(upgrade)) return;
        upgradesAtivos.Remove(upgrade);

        upgrade.Desativar();
    }

    public bool IsUpgradeAtivo(UpgradeObject upgrade) {
        return upgradesAtivos.Contains(upgrade);
    }

    public bool IsUpgradeComprado(UpgradeObject upgrade) {
        return upgradesComprados.Contains(upgrade);
    }

    public UpgradeObject CurrentOfSameType(UpgradeObject upgrade) {
        foreach (UpgradeObject up in upgradesAtivos) {
            if (upgrade.CalculoDeExclusividade(up)) {
                return up;
            }
        }

        return null;
    }

    public void ComprarUpgrade(UpgradeObject upgrade) {
        if (upgradesComprados.Contains(upgrade)) return;

        if (Player.instance.GetDinheiro() >= upgrade.custo) {
            Player.instance.RemoverDinheiro(upgrade.custo);
            upgradesComprados.Add(upgrade);
            upgrade.Ativar();
        }
    }

    #endregion

    void HabilitarGirar(float input) {
        if (input == 0) return;
        girando = true;
        girarValue = input;
    }

    void DesabilitarGirar() {
        girando = false;
        girarValue = 0;
    }

    void FixedUpdate() {
        if (girando) giradorRef.transform.Rotate(Vector3.up, girarValue * 30 * Time.fixedDeltaTime);
        else giradorRef.transform.Rotate(Vector3.up, giroPadrao * Time.fixedDeltaTime);
    }
}
