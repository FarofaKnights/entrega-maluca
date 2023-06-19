using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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


    #if UNITY_EDITOR
    [ContextMenu("Converter para timeline", true)]
    bool ValidateConvertToTimeline() {
        if (this.GetType() != typeof(MissaoTimeline)) return true;
        return false;
    }

    [ContextMenu("Converter para timeline")]
    void ConvertToTimeline() {
        List<TimelineElement> timeline = new List<TimelineElement>();
        TimelineElement comecoNaoSequencial = null;

        timeline.Add(new TimelineElement() {
            tipo = TimelineElementType.Objetivo,
            objetivo = objetivoInicial
        });

        if (objetivoInicial.diretriz != null && objetivoInicial.diretriz.texto != "") {
            timeline.Add(new TimelineElement() {
                tipo = TimelineElementType.Diretriz,
                diretriz = objetivoInicial.diretriz
            });
        }

        for (int i = 0; i < conjuntos.Length; i++) {
            ConjuntoObject conjunto = conjuntos[i];
            
            if (!conjunto.sequencial) {
                comecoNaoSequencial = new TimelineElement() {
                    tipo = TimelineElementType.ComecoNaoSequencial
                };
                timeline.Add(comecoNaoSequencial);
            }

            if (conjunto.diretriz != null && conjunto.diretriz.texto != "") {
                timeline.Add(new TimelineElement() {
                    tipo = TimelineElementType.Diretriz,
                    diretriz = conjunto.diretriz
                });
            }

            for (int j = 0; j < conjunto.objetivos.Length; j++) {
                ObjetivoObject objetivo = conjunto.objetivos[j];
                timeline.Add(new TimelineElement() {
                    tipo = TimelineElementType.Objetivo,
                    objetivo = objetivo
                });

                if (objetivo.diretriz != null && objetivo.diretriz.texto != "") {
                    timeline.Add(new TimelineElement() {
                        tipo = TimelineElementType.Diretriz,
                        diretriz = objetivo.diretriz
                    });
                }
            }


            if (!conjunto.sequencial) {
                TimelineElement fim = new TimelineElement() {
                    tipo = TimelineElementType.FimNaoSequencial
                };
                
                comecoNaoSequencial.naoSequencial = fim;
                fim.naoSequencial = comecoNaoSequencial;

                timeline.Add(fim);
            }
        }

        string path = AssetDatabase.GetAssetPath(this.GetInstanceID());
        string[] pathSplit = path.Split('/');
        string[] pathSplit2 = pathSplit[pathSplit.Length - 1].Split('.');
        pathSplit2[0] += "Timeline";
        pathSplit[pathSplit.Length - 1] = pathSplit2[0] + "." + pathSplit2[1];
        path = string.Join("/", pathSplit);
        Debug.Log(path);

        MissaoTimeline asset = ScriptableObject.CreateInstance<MissaoTimeline>();
        asset.timeline = timeline;
        asset.nome = nome;
        asset.descricao = descricao;
        
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

    }
    #endif
}
