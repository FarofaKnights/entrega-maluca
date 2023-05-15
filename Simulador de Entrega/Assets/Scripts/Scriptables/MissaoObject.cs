using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Missao", menuName = "Missao")]
public class MissaoObject : ScriptableObject {
    public string nome;
    public string descricao;

    public ObjetivoObject objetivoInicial;

    public ConjuntoObject[] conjuntos;

    public Missao Convert() {
        ObjetivoInicial objetivoInicial = new ObjetivoInicial(this.objetivoInicial.Convert());
        Conjunto[] conjuntos = new Conjunto[this.conjuntos.Length];

        for (int i = 0; i < conjuntos.Length; i++) {
            conjuntos[i] = this.conjuntos[i].Convert();
        }

        return new Missao(objetivoInicial, conjuntos, nome, descricao);
    }
}
