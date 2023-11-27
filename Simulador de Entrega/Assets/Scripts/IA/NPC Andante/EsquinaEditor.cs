#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodoIA))]
public class EsquinaEditor : Editor {

    public int id = 0;

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();

        NodoIA nodo = (NodoIA)target;

        if (GUILayout.Button("Add Esquina")) {
            AddEsquina(nodo);
        }

        // number field
        int oldId = id;
        id = EditorGUILayout.IntField("Esquina", id);

        if (GUILayout.Button("Mostrar selecionado") || oldId != id) {
            DrawSelectedLine();
        }

        if (GUILayout.Button("Add Intersecao")) {
            if (id > nodo.nodosConectados.Count) {
                Debug.LogError("Valor invalido");
                return;
            }

            NodoIA outroConectado = nodo.nodosConectados[id];
            AddIntersecao(nodo, outroConectado);
        }
    }

    void DrawSelectedLine(){
        NodoIA nodo = (NodoIA)target;
         if (id > nodo.nodosConectados.Count) return;
        NodoIA outroConectado = nodo.nodosConectados[id];

        Vector3 direccion = outroConectado.transform.position - nodo.transform.position;
        
        Vector3 centro = nodo.transform.position;
        Vector3 desenhar = nodo.transform.position + direccion.normalized * 0.1f * direccion.magnitude;

        centro.y += 0.5f;
        desenhar.y += 0.5f;

        Debug.DrawLine(centro, desenhar, Color.magenta, 0.1f, false);
    }

    GameObject AddEsquina(NodoIA nodo) {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(NodoIA.prefabNodo);
        GameObject novoNodo = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        novoNodo.transform.position = nodo.transform.position;

        novoNodo.transform.SetParent(nodo.transform.parent);

        // adicionar a lista de nodos conectados
        nodo.nodosConectados.Add(novoNodo.GetComponent<NodoIA>());
        novoNodo.GetComponent<NodoIA>().nodosConectados.Add(nodo);

        // selecionar a nova esquina
        Selection.activeGameObject = novoNodo;

        return novoNodo;
    }

    GameObject AddIntersecao(NodoIA nodo, NodoIA outroConectado) {
        Vector3 pos = (nodo.transform.position + outroConectado.transform.position) * 0.5f;
        GameObject novo = AddEsquina(nodo);
        NodoIA novoNodo = novo.GetComponent<NodoIA>();

        novoNodo.transform.position = pos;
        novoNodo.nodosConectados.Add(outroConectado);

        nodo.nodosConectados.Remove(outroConectado);
        nodo.nodosConectados.Add(novoNodo);
        outroConectado.nodosConectados.Remove(nodo);
        outroConectado.nodosConectados.Add(novoNodo);

        return novo;
    }
}
#endif