using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton: MonoBehaviour {
    public UpgradeObject upgradeObject;
    public int id = 0;

    private void Start() {
        OficinaController.instance.upgrades.Add(this.upgradeObject);
    }
    public void Comprar() {
        upgradeObject.Comprar();
    }
}
