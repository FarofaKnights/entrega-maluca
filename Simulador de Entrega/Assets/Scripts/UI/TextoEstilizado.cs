using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextoEstilizado : MonoBehaviour
{

    void CriarTexto()
    {
        Text texto = GetComponent<Text>();
        texto.enabled = true;

        // Delete all children
        foreach (Transform child in transform) {
            DestroyImmediate(child.gameObject);
        }

        // Make sure all children are deleted
        if (transform.childCount > 0) {
            foreach (Transform child in transform) {
                DestroyImmediate(child.gameObject);
            }
        }
        
        GameObject sombra2 = Instantiate(gameObject);
        GameObject sombra1 = Instantiate(gameObject);
        GameObject textoPrincipal = Instantiate(gameObject);

        sombra2.transform.position = new Vector3(-5f, 5f, 0);
        sombra1.transform.position = new Vector3(-2.5f, 2.5f, 0);
        textoPrincipal.transform.position = Vector3.zero;

        sombra2.transform.SetParent(transform, false);
        sombra1.transform.SetParent(transform, false);
        textoPrincipal.transform.SetParent(transform, false);

        // Melhorar isso
        sombra2.GetComponent<Text>().color = new Color(0.192f, 0, 0.282f);
        sombra1.GetComponent<Text>().color = new Color(0.863f, 0.31f, 0);
        textoPrincipal.GetComponent<Text>().color = new Color(0.859f, 1, 0);

        Outline outline = textoPrincipal.AddComponent<Outline>();
        outline.effectColor = Color.black;

        texto.enabled = false;
    }

    [ContextMenu("Gerar texto")]
    void CreateText()
    {
        CriarTexto();
    }
}
