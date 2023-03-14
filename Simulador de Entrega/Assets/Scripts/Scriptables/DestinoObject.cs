using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DestinoObject {
    public string endereco;
    public List<CargaObject> cargas;
    public bool permiteReceber = false;
}
