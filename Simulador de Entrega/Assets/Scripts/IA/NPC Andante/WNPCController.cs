using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNPCController : MonoBehaviour {
    public static WNPCController instance;

    public List<NodoIA> nodosVisitaveis = new List<NodoIA>();

    public GameObject prefab;
    public int quantidade;
    public GameObject esquinasHolder;

    void Awake() {
        instance = this;
    }

    void Start() {
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
        GameObject npc = Instantiate(prefab, nodoInicial.transform.position, Quaternion.identity);
        npc.transform.SetParent(transform, true);
        
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
        Debug.Log("Random index: " + randomIndex + " / " + nodosVisitaveis.Count);
        return nodosVisitaveis[randomIndex];
    }
}
