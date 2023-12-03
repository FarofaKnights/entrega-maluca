using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoIA : MonoBehaviour{
    public static string prefabNodo = "Assets/Prefabs/IA/Esquina.prefab";

    public List<NodoIA> nodosConectados = new List<NodoIA>();
    public bool visitavel = false;
    public bool descanso = false;

    WNPCController controller;

    void Awake() {
        controller = GetComponentInParent<WNPCController>();
    }

    #if UNITY_EDITOR

    void OnDrawGizmos(){
        Color cor = Color.red;
        float tamanho = 1.5f;

        if (visitavel) {
            cor = Color.magenta;
            tamanho = 3f;
        } else if (descanso) {
            cor = Color.blue;
            tamanho = 2f;
        }
        
        Gizmos.color = cor;
        Gizmos.DrawWireSphere(transform.position, tamanho);

        Gizmos.color = Color.green;
        foreach(NodoIA nodo in nodosConectados){
            if (nodo == null) continue;
            Vector3 direccion = nodo.transform.position - transform.position;
            float dist = direccion.magnitude;

            Vector3 desenharMetade = transform.position + (direccion * 0.5f);

            // Drawline
            Gizmos.DrawLine(transform.position, desenharMetade);
        }
    }

    #endif

    void OnDestroy() {
        if (!gameObject.scene.isLoaded) return;
        controller.nodosVisitaveis.Remove(this);

        foreach(NodoIA nodo in nodosConectados){
            nodo.nodosConectados.Remove(this);
        }
    }
}
