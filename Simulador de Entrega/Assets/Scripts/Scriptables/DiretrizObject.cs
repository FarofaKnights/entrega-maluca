using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiretrizObject {
    public string texto;
    public List<TimerObject> limitacoes;

    public Diretriz Convert(int nivel = 2) {
        if (limitacoes == null || limitacoes.Count == 0)
            return new Diretriz(texto, nivel);

        List<Limitacao> limitacoesConvertidas = new List<Limitacao>();
        foreach (TimerObject timer in limitacoes) {
            limitacoesConvertidas.Add(timer.Convert());
        }

        return new Diretriz(texto, limitacoesConvertidas.ToArray(), nivel);
    }

}
