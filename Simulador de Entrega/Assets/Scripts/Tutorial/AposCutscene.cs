using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AposCutscene : MonoBehaviour {
    public GameObject objeto;
    public bool estado;
    void Start() {
        CutsceneController.instance.aoTerminar += AtualizarObjeto;
    }

    public void AtualizarObjeto() {
        objeto.SetActive(estado);

        CutsceneController.instance.aoTerminar -= AtualizarObjeto;
    }
}
