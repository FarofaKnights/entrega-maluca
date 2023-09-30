using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNPCController : MonoBehaviour {
    public static WNPCController instance;

    public GameObject prefab;
    public int quantidade;
    public GameObject esquinasHolder;
    public bool showGizmos = true;

    void Awake() {
        instance = this;
    }

    void Start() {
        for (int i = 0; i < quantidade; i++) {
            GerarWNPC();
        }
    }

    public void GerarWNPC() {
        GameObject esquina = GetRandomEsquina();
        GerarWNPC(esquina);
    }

    public void GerarWNPC(GameObject nodoInicial) {
        GameObject npc = Instantiate(prefab, nodoInicial.transform.position, Quaternion.identity);
        
        WNPCMachine wnpc = npc.GetComponent<WNPCMachine>();
        wnpc.SetTarget(nodoInicial.transform);
    }

    GameObject GetRandomEsquina() {
        int randomIndex = Random.Range(0, esquinasHolder.transform.childCount);
        return esquinasHolder.transform.GetChild(randomIndex).gameObject;
    }

    void FixedUpdate() {
        List<WNPCMachine> npcs = new List<WNPCMachine>(GameObject.FindObjectsOfType<WNPCMachine>());
        int quantidadeNpcs = npcs.Count;

        if (quantidadeNpcs < quantidade) {
            GerarWNPC();
        } else if (quantidadeNpcs > quantidade) {
            WNPCMachine npc = npcs[Random.Range(0, npcs.Count)];
            Destroy(npc.gameObject);
        }
    }
}
