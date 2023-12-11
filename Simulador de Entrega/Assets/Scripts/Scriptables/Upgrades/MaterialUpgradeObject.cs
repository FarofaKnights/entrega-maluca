using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Material tal", menuName = "Entrega Maluca/Upgrade/Material"), System.Serializable]
public class MaterialUpgradeObject : UpgradeObject {
    public Material material;

    protected override void _Ativar() {
        OficinaController.instance.SetMaterial(material);
    }

    protected override void _Desativar() {
        OficinaController.instance.SetMaterial(null);
    }

    public override Dictionary<string, (string, float)> GetInfo() {
        return null;
    }

    public override bool CalculoDeExclusividade(UpgradeObject outro) {
        return outro is MaterialUpgradeObject;
    }
}
