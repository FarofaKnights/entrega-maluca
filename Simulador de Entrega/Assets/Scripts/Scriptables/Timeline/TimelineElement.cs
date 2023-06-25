using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimelineElementType { Objetivo, Diretriz, ComecoNaoSequencial, FimNaoSequencial, Cutscene}

[System.Serializable]
public class TimelineElement {
    public TimelineElement naoSequencial;
    public TimelineElementType tipo;
    public ObjetivoObject objetivo;
    public DiretrizObject diretriz;
    public List<CutsceneObject> cutscenes;
    public bool mostrandoFilhos = true;
}
