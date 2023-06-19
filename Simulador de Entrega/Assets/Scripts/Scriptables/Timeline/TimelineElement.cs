using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimelineElementType { Objetivo, Diretriz, ComecoNaoSequencial, FimNaoSequencial}

[System.Serializable]
public class TimelineElement {
    public TimelineElement naoSequencial;
    public TimelineElementType tipo;
    public ObjetivoObject objetivo;
    public DiretrizObject diretriz;
    public bool mostrandoFilhos = true;
}
