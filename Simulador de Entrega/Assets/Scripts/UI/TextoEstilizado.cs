using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextoEstilizado : MonoBehaviour
{

    void CriarTexto()
    {
        Text texto = GetComponent<Text>();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        GameObject sombra2 = ClonarTexto();
        GameObject sombra1 = ClonarTexto();
        GameObject textoPrincipal = ClonarTexto();

        sombra2.GetComponent<Text>().color = new Color(49, 0, 72);
        sombra1.GetComponent<Text>().color = new Color(220, 79, 0);
        textoPrincipal.GetComponent<Text>().color = new Color(219, 255, 0);

        texto.enabled = false;
    }

    GameObject ClonarTexto()
    {
        GameObject textPrincipal = Instantiate(gameObject);
        textPrincipal.transform.SetParent(gameObject.transform);
        textPrincipal.transform.position = transform.position;
        textPrincipal.transform.localScale = new Vector3(1, 1, 1);
        return textPrincipal;
    }

    [ContextMenu("Gerar texto")]
    void CreateText()
    {
        CriarTexto();
    }
}
