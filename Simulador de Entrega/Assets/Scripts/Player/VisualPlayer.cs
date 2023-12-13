using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialLocation {
    public GameObject obj;
    public int materialIndex;
    Material materialInicial;

    public void SetMaterialInicial() {
        materialInicial = obj.GetComponent<Renderer>().materials[materialIndex];
    }

    public void ResetMaterialToInicial() {
        SetMaterial(materialInicial);
    }

    public void SetMaterial(Material mat) {
        Material[] materials = obj.GetComponent<Renderer>().materials;
        materials[materialIndex] = mat;
        
        obj.GetComponent<Renderer>().materials = materials;
    }
}

public class VisualPlayer : MonoBehaviour {
    public static VisualPlayer instance;

    public enum Localizacao { Chapeu, Frontal, Traseira, Visao }
    public enum SkinLocation { None, Cabine, Capo, Porta, Luzes, Frente, Cacamba }

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

    void Start() {
        MaterialLocation[][] locations = new MaterialLocation[][] {
            materialsInfo.cabineMaterials,
            materialsInfo.capoMaterial,
            materialsInfo.portaMaterial,
            materialsInfo.luzesMaterial,
            materialsInfo.frenteMaterial,
            materialsInfo.cacambaMaterial
        };

        foreach (MaterialLocation[] item in locations) {
            foreach (MaterialLocation materialLocation in item) {
                materialLocation.SetMaterialInicial();
            }
        }
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

    public void SetSkin(Material material, SkinLocation location) {
        MaterialLocation[] locations = null;

        switch (location) {
            case SkinLocation.Cabine:
                locations = materialsInfo.cabineMaterials;
                break;
            case SkinLocation.Capo:
                locations = materialsInfo.capoMaterial;
                break;
            case SkinLocation.Porta:
                locations = materialsInfo.portaMaterial;
                break;
            case SkinLocation.Luzes:
                locations = materialsInfo.luzesMaterial;
                break;
            case SkinLocation.Frente:
                locations = materialsInfo.frenteMaterial;
                break;
            case SkinLocation.Cacamba:
                locations = materialsInfo.cacambaMaterial;
                break;
        }

        foreach (MaterialLocation item in locations) {
            item.SetMaterial(material);
        }
    }

    public void RemoveSkin(SkinLocation location) {
        MaterialLocation[] locations = null;

        switch (location) {
            case SkinLocation.Cabine:
                locations = materialsInfo.cabineMaterials;
                break;
            case SkinLocation.Capo:
                locations = materialsInfo.capoMaterial;
                break;
            case SkinLocation.Porta:
                locations = materialsInfo.portaMaterial;
                break;
            case SkinLocation.Luzes:
                locations = materialsInfo.luzesMaterial;
                break;
            case SkinLocation.Frente:
                locations = materialsInfo.frenteMaterial;
                break;
            case SkinLocation.Cacamba:
                locations = materialsInfo.cacambaMaterial;
                break;
        }

        foreach (MaterialLocation item in locations) {
            item.ResetMaterialToInicial();
        }
    }

    public static SkinLocation StringToSkinLocation(string txt) {
        switch (txt) {
            case "cabine":
                return SkinLocation.Cabine;
            case "capo":
                return SkinLocation.Capo;
            case "porta":
                return SkinLocation.Porta;
            case "luzes":
                return SkinLocation.Luzes;
            case "frente":
                return SkinLocation.Frente;
            case "cacamba":
                return SkinLocation.Cacamba;
            default:
                return SkinLocation.None;
        }
    }
}
