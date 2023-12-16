using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Material tal", menuName = "Entrega Maluca/Upgrade/Material"), System.Serializable]
public class MaterialUpgradeObject : UpgradeObject {
    public Material material;
    public Color colorIcone = Color.white;

    protected override void _Ativar() { }

    protected override void _Desativar() { }

    public override Dictionary<string, (string, float)> GetInfo() {
        return null;
    }

    public override bool CalculoDeExclusividade(UpgradeObject outro) {
        return outro is MaterialUpgradeObject;
    }
}
