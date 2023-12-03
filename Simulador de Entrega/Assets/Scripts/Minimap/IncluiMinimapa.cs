using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncluiMinimapa : MonoBehaviour {
    public Sprite sprite;
    public bool comecaAtivo = false;
    public float escala = 1;

    bool ativo = false;
    GameObject icon = null;

    void Start() {
        GeraIcone();
    }

    public void GeraIcone() {
        if (this.icon != null) return;

        MinimapManager minimapManager = MinimapManager.instance;
        GameObject icon = Instantiate(minimapManager.iconPrefab, minimapManager.transform);
        icon.GetComponent<SpriteRenderer>().sprite = sprite;
        icon.GetComponent<MinimapIcon>().target = transform;
        icon.transform.localScale = new Vector3(escala, escala, escala);

        this.icon = icon;

        icon.SetActive(comecaAtivo);
        ativo = comecaAtivo;
    }

    public void AtivarIcone() {
        if (ativo) return;
        if(icon == null) GeraIcone();
        ativo = true;

        icon.SetActive(true);
    }

    public void DesativarIcone() {
        if (!ativo) return;
        if(icon == null) GeraIcone();
        ativo = false;

        icon.SetActive(false);
    }

    public void SetColor(Color color) {
        if(icon == null) GeraIcone();
        icon.GetComponent<SpriteRenderer>().color = color;
    }

    void OnDestroy() {
        if (icon != null) Destroy(icon);
    }
}
