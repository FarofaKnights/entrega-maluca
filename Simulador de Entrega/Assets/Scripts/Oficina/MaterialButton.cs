using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour {
    public MaterialUpgradeObject upgradeObject;
    public int id = 0;
    public GameObject locked;
    public RawImage icone;

    public void SetUpgrade(MaterialUpgradeObject upgradeObject) {
        if (upgradeObject == null) return;

        this.upgradeObject = upgradeObject;

        if (upgradeObject.iconeTextura != null) icone.texture = upgradeObject.iconeTextura;
        else icone.color = upgradeObject.colorIcone;

        locked.SetActive(!upgradeObject.comprado);
    }

    public void HandleSelecionar() {
        if (upgradeObject == null || upgradeObject.comprado) UIController.oficina.SetarCor(upgradeObject);
        else UIController.oficina.ShowDetalhes(this);
    }
}
