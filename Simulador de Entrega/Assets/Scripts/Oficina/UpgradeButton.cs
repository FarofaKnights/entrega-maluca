using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton: MonoBehaviour {
    public UpgradeObject upgradeObject;
    public int id = 0;

    public void Comprar() {
        upgradeObject.Comprar();
    }

    public void HandleSelecionar() {
        UIController.oficina.ShowDetalhes(this);
    }
}
