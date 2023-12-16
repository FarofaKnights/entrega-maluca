using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNPCController : MonoBehaviour {
    public static WNPCController instance;

    public List<NodoIA> nodosVisitaveis = new List<NodoIA>();

    WNPCFactory factory;
    public int quantidade;
    public GameObject esquinasHolder;
    public GameObject holder;

    void Awake() {
        instance = this;
    }

    void Start() {
        factory = GetComponent<WNPCFactory>();
        UpdateVisitaveisList();
        for (int i = 0; i < quantidade; i++) {
            GerarWNPC();
        }
    }

    void UpdateVisitaveisList() {
        nodosVisitaveis.Clear();
        foreach (Transform child in esquinasHolder.transform) {
            NodoIA nodo = child.GetComponent<NodoIA>();
            if (nodo == null) continue;
            if (!nodo.visitavel) continue;
            nodosVisitaveis.Add(nodo);
        }
    }

    public void GerarWNPC() {
        GameObject esquina = GetRandomEsquina();
        GerarWNPC(esquina);
    }

    public void GerarWNPC(GameObject nodoInicial) {
        GameObject npc = Instantiate(factory.GetRandomPrefab(), nodoInicial.transform.position, Quaternion.identity);
        npc.transform.localPosition = Vector3.zero;
        npc.transform.localRotation = Quaternion.identity;
        npc.transform.SetParent(holder.transform, true);
        
        WNPCMachine wnpc = npc.GetComponent<WNPCMachine>();
        wnpc.controller = this;
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

    public List<NodoIA> GetNodosVisitaveis(){
        return nodosVisitaveis;
    }

    public NodoIA GetRandomNodo(){
        int randomIndex = Random.Range(0, nodosVisitaveis.Count);
        return nodosVisitaveis[randomIndex];
    }
}
