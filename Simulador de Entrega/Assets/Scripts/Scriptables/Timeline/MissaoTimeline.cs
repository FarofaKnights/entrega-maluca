using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Missao Timeline", menuName = "Missao Timeline")]
public class MissaoTimeline : MissaoObject {
    public List<TimelineElement> timeline;
    
    [System.NonSerialized] public string mensagemTimeline = "";
    [System.NonSerialized] public System.DateTime lastSaveTime;

    [ContextMenu("(Des)abilitar timeline")]
    void ToggleTimeline() {
        mensagemTimeline = "Alternar";
    }

    [ContextMenu("Atualizar timeline")]
    void RefreshTimeline() {
        mensagemTimeline = "Atualizar";
    }
    

    public bool CheckValidade() {
        bool abriuNaoSequencial = false;

        for (int i = 0; i < timeline.Count; i++) {
            TimelineElement element = timeline[i];

            if (element.tipo == TimelineElementType.ComecoNaoSequencial) {
                if (abriuNaoSequencial) return false;
                abriuNaoSequencial = true;
            } else if (element.tipo == TimelineElementType.FimNaoSequencial) {
                if (!abriuNaoSequencial) return false;
                abriuNaoSequencial = false;
            }
        }

        if (abriuNaoSequencial) return false;

        return true;
    }

    public void TimelineToMissao() {
        if (timeline.Count == 0 || !CheckValidade()) return;

        List<ConjuntoObject> conjuntos = new List<ConjuntoObject>();
        List<ObjetivoObject> objetivos = new List<ObjetivoObject>();

        ConjuntoObject conjunto = new ConjuntoObject();
        conjunto.sequencial = true;

        ObjetivoObject objetivoInicial = null, ultimoObjetivo = null;
        string ultimoElemento = "conjunto"; // "conjunto", "objetivo"

        for (int i = 0; i < timeline.Count; i++) {
            TimelineElement element = timeline[i];
            if (element.tipo == TimelineElementType.ComecoNaoSequencial) {
                conjunto.objetivos = objetivos.ToArray();
                conjuntos.Add(conjunto);
                objetivos = new List<ObjetivoObject>();
                conjunto = new ConjuntoObject();
                conjunto.sequencial = false;
                ultimoElemento = "conjunto";
            } else if (element.tipo == TimelineElementType.FimNaoSequencial) {
                conjunto.objetivos = objetivos.ToArray();
                conjuntos.Add(conjunto);
                objetivos = new List<ObjetivoObject>();
                conjunto = new ConjuntoObject();
                conjunto.sequencial = true;
                ultimoElemento = "conjunto";
            } else if (element.tipo == TimelineElementType.Objetivo) {
                ObjetivoObject objetivo = new ObjetivoObject();
                objetivo.endereco = element.objetivo.endereco;
                objetivo.cargas = element.objetivo.cargas;
                objetivo.permiteReceber = element.objetivo.permiteReceber;

                if (objetivoInicial == null) objetivoInicial = objetivo;
                else objetivos.Add(objetivo);

                ultimoObjetivo = objetivo;
                ultimoElemento = "objetivo";
            } else if (element.tipo == TimelineElementType.Diretriz) {
                if (ultimoElemento == "conjunto") conjunto.diretriz = element.diretriz;
                else if (ultimoElemento == "objetivo") ultimoObjetivo.diretriz = element.diretriz;
            }
        }

        if (!conjuntos.Contains(conjunto)) {
            conjunto.objetivos = objetivos.ToArray();
            conjuntos.Add(conjunto);
        }

        objetivos = null;

        for (int i = conjuntos.Count - 1; i >= 0; i--) {
            if (conjuntos[i].objetivos.Length == 0) conjuntos.RemoveAt(i);
        }

        this.objetivoInicial = objetivoInicial;
        this.conjuntos = conjuntos.ToArray();

        lastSaveTime = System.DateTime.Now;
    }
}
