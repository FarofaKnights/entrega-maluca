using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMotor : IUpgrade {
    public int maxSpeed;
    public float acelleration;

    public override void Ativar() {
        if (!comprado || ativo) return;

        DesativarOutrosUpgrades();

        ativo = true;
        OficinaController.instance.SetMotor(maxSpeed, acelleration);
    }

    public override void Desativar() {
        if (!comprado || !ativo) return;

        ativo = false;
        OficinaController.instance.SetMotor(-1, -1);
    }

    void DesativarOutrosUpgrades() {
        foreach (IUpgrade u in OficinaController.instance.upgradesComprados) {
            if (u != this && u is UpgradeMotor) {
                u.Desativar();
            }
        }
    }
}
