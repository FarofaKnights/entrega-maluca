using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnderecoFalso: Endereco {
    void Awake() {
        if (!ListaEnderecos.ContainsKey("LugarNenhum")) {
            ListaEnderecos.Add("LugarNenhum", this);
        }
    }

    public override void DefinirComoObjetivo(Objetivo objetivo) {}
    public override void RemoverObjetivo() {}
}