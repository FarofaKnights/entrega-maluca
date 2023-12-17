using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton: MonoBehaviour {
    public UpgradeObject upgradeObject;
    public int id = 0;

    public Text text;
    public Image icone;
    public GameObject locked;

    public void SetUpgrade(UpgradeObject upgradeObject) {
        if (upgradeObject == null) return;

        this.upgradeObject = upgradeObject;

        if (upgradeObject.icone != null) {
            icone.sprite = upgradeObject.icone;
            text.gameObject.SetActive(false);
            icone.gameObject.SetActive(true);
        } else {
            text.text = upgradeObject.nome;
            text.gameObject.SetActive(true);
            icone.gameObject.SetActive(false);
        }

        locked.SetActive(!upgradeObject.comprado);
    }


    public void Comprar() {
        upgradeObject.Comprar();
    }

    public void HandleSelecionar() {
        UIController.oficina.ShowDetalhes(this);
    }
}
