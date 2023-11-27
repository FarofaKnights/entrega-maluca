#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WNPCController))]
public class WNPCControllerEditor : Editor {
    int corrigidas = 0;
    int nulas = 0;

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();

        WNPCController controller = (WNPCController) target;

        if (GUILayout.Button("Corrigir ligações")) {
            corrigidas = 0;
            nulas = 0;

            foreach (NodoIA nodo in GetNodos(controller)) {
                CorrigirLigacao(nodo);
            }

            Debug.Log("Ligações corrigidas: " + corrigidas);
            Debug.Log("Ligações nulas removidas: " + nulas);
        }
    }

    public List<NodoIA> GetNodos(WNPCController controller){
        List<NodoIA> nodos = new List<NodoIA>();
        foreach (Transform child in controller.esquinasHolder.transform) {
            NodoIA nodo = child.GetComponent<NodoIA>();
            if (nodo == null) continue;
            nodos.Add(nodo);
        }
        return nodos;
    }

    public void CorrigirLigacao(NodoIA nodo) {
        int i = -1;
        List<int> indicesNulos = new List<int>();
        foreach(NodoIA outroNodo in nodo.nodosConectados){
            if (outroNodo == null) {
                indicesNulos.Add(i);
                continue;
            }
            if (outroNodo.nodosConectados.Contains(nodo)) continue;

            outroNodo.nodosConectados.Add(nodo);
            corrigidas++;
        }

        foreach(int index in indicesNulos){
            nodo.nodosConectados.RemoveAt(index);
            nulas++;
        }
    }
}
#endif