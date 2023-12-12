using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialLocation {
    public GameObject obj;
    public int materialIndex;
}

public class VisualPlayer : MonoBehaviour {
    public static VisualPlayer instance;

    public enum Localizacao { Chapeu, Frontal, Traseira, Visao }

    public GameObject chapeuHolder;
    public GameObject frontalHolder;
    public GameObject traseiraHolder;
    public GameObject visaoHolder;

    [System.Serializable]
    class MaterialsInfo {
        public MaterialLocation[] cabineMaterials;
        public MaterialLocation[] capoMaterial;
        public MaterialLocation[] portaMaterial;
        public MaterialLocation[] luzesMaterial;
        public MaterialLocation[] frenteMaterial;
        public MaterialLocation[] cacambaMaterial;
    }

    
    [SerializeField] MaterialsInfo materialsInfo;


    void Awake() {
        instance = this;
    }

    public void SetAcessorio(GameObject acessorioPrefab, Localizacao localizacao) {
        GameObject holder = null;

        switch (localizacao) {
            case Localizacao.Chapeu:
                holder = chapeuHolder;
                break;
            case Localizacao.Frontal:
                holder = frontalHolder;
                break;
            case Localizacao.Traseira:
                holder = traseiraHolder;
                break;
            case Localizacao.Visao:
                holder = visaoHolder;
                break;
        }

        if (holder.transform.childCount > 0) {
            Destroy(holder.transform.GetChild(0).gameObject);
        }

        GameObject skin = Instantiate(acessorioPrefab, holder.transform);
    }

    public void RemoveAcessorio(Localizacao localizacao) {
        GameObject holder = null;

        switch (localizacao) {
            case Localizacao.Chapeu:
                holder = chapeuHolder;
                break;
            case Localizacao.Frontal:
                holder = frontalHolder;
                break;
            case Localizacao.Traseira:
                holder = traseiraHolder;
                break;
            case Localizacao.Visao:
                holder = visaoHolder;
                break;
        }

        if (holder.transform.childCount > 0) {
            Destroy(holder.transform.GetChild(0).gameObject);
        }
    }
}
