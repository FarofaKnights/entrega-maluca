using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMaterial : IUpgrade {
    public Material material;

    public override void Ativar() {
        if (!comprado || ativo) return;

        DesativarOutrosUpgrades();

        ativo = true;
        OficinaController.instance.SetMaterial(material);
    }

    public override void Desativar() {
        if (!comprado || !ativo) return;

        ativo = false;
        OficinaController.instance.SetMaterial(null);
    }

    void DesativarOutrosUpgrades() {
        foreach (IUpgrade u in OficinaController.instance.upgradesComprados) {
            if (u != this && u is UpgradeMaterial) {
                u.Desativar();
            }
        }
    }
}
