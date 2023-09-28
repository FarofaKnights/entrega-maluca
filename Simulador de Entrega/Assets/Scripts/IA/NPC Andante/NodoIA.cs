using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoIA : MonoBehaviour{
    static List<NodoIA> nodosVisitaveis = new List<NodoIA>();

    public List<NodoIA> nodosConectados = new List<NodoIA>();
    public bool visitavel = false;

    void Awake() {
        if (visitavel) {
            nodosVisitaveis.Add(this);
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);

        Gizmos.color = Color.green;
        foreach(NodoIA nodo in nodosConectados){
            Vector3 direccion = nodo.transform.position - transform.position;
            float dist = direccion.magnitude;

            Vector3 desenharMetade = transform.position + (direccion * 0.5f);

            // Drawline
            Gizmos.DrawLine(transform.position, desenharMetade);
        }
    }

    public static List<NodoIA> GetNodosVisitaveis(){
        return nodosVisitaveis;
    }

    public static NodoIA GetRandomNodo(){
        int randomIndex = Random.Range(0, nodosVisitaveis.Count);
        Debug.Log("Random index: " + randomIndex);
        return nodosVisitaveis[randomIndex];
    }
}
