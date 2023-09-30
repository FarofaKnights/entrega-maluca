using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoIA : MonoBehaviour{
    static List<NodoIA> nodosVisitaveis = new List<NodoIA>();

    public List<NodoIA> nodosConectados = new List<NodoIA>();
    public bool visitavel = false;
    public bool descanso = false;

    void Awake() {
        if (visitavel) {
            nodosVisitaveis.Add(this);
        }
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
            Vector3 direccion = nodo.transform.position - transform.position;
            float dist = direccion.magnitude;

            Vector3 desenharMetade = transform.position + (direccion * 0.5f);

            // Drawline
            Gizmos.DrawLine(transform.position, desenharMetade);
        }
    }

    #endif

    public static List<NodoIA> GetNodosVisitaveis(){
        return nodosVisitaveis;
    }

    public static NodoIA GetRandomNodo(){
        int randomIndex = Random.Range(0, nodosVisitaveis.Count);
        Debug.Log("Random index: " + randomIndex);
        return nodosVisitaveis[randomIndex];
    }
}
