using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiretrizObject {
    public string texto;

    public Diretriz Convert() {
        return new Diretriz(texto);
    }

}
