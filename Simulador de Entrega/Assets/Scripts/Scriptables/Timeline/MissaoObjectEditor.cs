#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum TimelineElements { Objetivo, Diretriz, Sequencial };

[CustomEditor(typeof(MissaoTimeline))]
public class MissaoObjectEditor : Editor {
    MissaoTimeline missaoObject;

    bool erroNaoSequencial = false;
    bool showingTimeline = true;

    public int padding = 4;

    List<TimelineElement> elementMoveUp = new List<TimelineElement>();
    List<TimelineElement> elementMoveDown = new List<TimelineElement>();
    List<TimelineElement> elementRemove = new List<TimelineElement>();

    string chamouPicker = "";


    public void OnEnable() {
        missaoObject = (MissaoTimeline)target;
    }

    void HandleMensagens() {
        if (missaoObject.mensagemTimeline == "Alternar") {
            showingTimeline = !showingTimeline;
            missaoObject.mensagemTimeline = "";
        } else if (missaoObject.mensagemTimeline == "Atualizar") {
            if (!erroNaoSequencial && missaoObject.CheckValidade()) missaoObject.TimelineToMissao();
            else Debug.Log("Timeline inválida");
            missaoObject.mensagemTimeline = "";
        }
    }

    public override void OnInspectorGUI() {
        HandleMensagens();

        if (!showingTimeline) {
            DrawDefaultInspector();
            return;
        }
        
        Undo.RecordObject(missaoObject, "MissaoTimeline");

        bool mostrar = true;

        EditorGUI.BeginChangeCheck();
        missaoObject.nome = EditorGUILayout.TextField("Nome:", missaoObject.nome);
        
        EditorGUILayout.LabelField("Descrição:");
        missaoObject.descricao = EditorGUILayout.TextArea(missaoObject.descricao, GUILayout.Height(40));

        EditorGUILayout.LabelField("Timeline:");

        // Create a panel for the timeline
        GUILayout.BeginVertical("box");
        GUILayout.Space(padding);

        Queue<TimelineElement> comecoNaoSequencial = new Queue<TimelineElement>();

        if (missaoObject.timeline == null) missaoObject.timeline = new List<TimelineElement>();

        // Draw each element of the timeline
        int counter = 0;
        foreach (TimelineElement element in missaoObject.timeline) {
            counter++;

            if (element.tipo == TimelineElementType.FimNaoSequencial && !mostrar) {
                mostrar = true;
                if (GUI.backgroundColor == Color.cyan) GUI.backgroundColor = Color.white;
                if (GUI.backgroundColor == Color.red) GUI.backgroundColor = Color.cyan;

                TimelineElement comeco = comecoNaoSequencial.Dequeue();
                element.naoSequencial = comeco;
                comeco.naoSequencial = element;
                continue;
            }
            if (!mostrar) continue;

            EditorGUILayout.BeginHorizontal();
            // spacing
            GUILayout.Space(padding);

            if (element.tipo == TimelineElementType.FimNaoSequencial && comecoNaoSequencial.Count > 0) {
                TimelineElement comeco = comecoNaoSequencial.Dequeue();
                element.naoSequencial = comeco;
                comeco.naoSequencial = element;
            }

            for (int i = 0; i < comecoNaoSequencial.Count; i++) {
                GUILayout.Space(padding);
                GUILayout.Space(padding);
            }
            

            switch (element.tipo) {
                case TimelineElementType.Objetivo:
                    DrawObjetivo(element, counter);
                    break;
                case TimelineElementType.Diretriz:
                    DrawDiretriz(element);
                    break;
                case TimelineElementType.ComecoNaoSequencial:
                    DrawNaoSequencialInicio(element);
                    comecoNaoSequencial.Enqueue(element);
                    mostrar = element.mostrandoFilhos;
                    break;
                case TimelineElementType.FimNaoSequencial:
                    DrawNaoSequencialFim(element);
                    break;
            }

            GUILayout.Space(padding);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(padding/2.0f);
        }
        GUILayout.Space(padding/2.0f);
        // End panel
        GUILayout.EndVertical();

        if (GUILayout.Button("Adicionar Elemento")) {
            // Open context menu with TimelineElements options
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Objetivo"), false, AddObjetivo);
            menu.AddItem(new GUIContent("Diretriz"), false, AddDiretriz);
            menu.AddItem(new GUIContent("Não Sequencial"), false, AddNaoSequencial);
            menu.ShowAsContext();

        }

        if (erroNaoSequencial) {
            EditorGUILayout.HelpBox("Não é possível adicionar elementos sequenciais dentro de um elemento não sequencial.", MessageType.Error);
        }

        // Move up and down buttons
        if (elementMoveUp.Count > 0) {
            foreach (TimelineElement element in elementMoveUp) {
                int index = missaoObject.timeline.IndexOf(element);
                if (index > 0) {
                    TimelineElement anterior = missaoObject.timeline[index - 1];
                    if (anterior.tipo == TimelineElementType.FimNaoSequencial && !anterior.naoSequencial.mostrandoFilhos) {
                        int indexComeco = missaoObject.timeline.IndexOf(anterior.naoSequencial);
                        for (int i = index; i > indexComeco; i--) {
                            TimelineElement element2 = missaoObject.timeline[i];
                            missaoObject.timeline[i] = missaoObject.timeline[i - 1];
                            missaoObject.timeline[i - 1] = element2;
                        }
                    } else {
                        missaoObject.timeline[index] = missaoObject.timeline[index - 1];
                        missaoObject.timeline[index - 1] = element;
                    }
                }
            }
            elementMoveUp.Clear();
        }

        if (elementMoveDown.Count > 0) {
            for (int i = elementMoveDown.Count - 1; i >= 0; i--) {
                TimelineElement element = elementMoveDown[i];
                int index = missaoObject.timeline.IndexOf(element);
                if (index < missaoObject.timeline.Count - 1) {
                    TimelineElement proximo = missaoObject.timeline[index + 1];
                    if (proximo.tipo == TimelineElementType.ComecoNaoSequencial && !proximo.mostrandoFilhos) {
                        int indexFim = missaoObject.timeline.IndexOf(proximo.naoSequencial);
                        for (int j = index; j < indexFim; j++) {
                            TimelineElement element2 = missaoObject.timeline[j];
                            missaoObject.timeline[j] = missaoObject.timeline[j + 1];
                            missaoObject.timeline[j + 1] = element2;
                        }
                    } else {
                        missaoObject.timeline[index] = missaoObject.timeline[index + 1];
                        missaoObject.timeline[index + 1] = element;
                    }
                }
            }
            elementMoveDown.Clear();
        }

        // Remove buttons
        if (elementRemove.Count > 0) {
            foreach (TimelineElement element in elementRemove) {
                missaoObject.timeline.Remove(element);
            }
            elementRemove.Clear();
        }

        // Save changes
        if (EditorGUI.EndChangeCheck()){
            EditorUtility.SetDirty(missaoObject);
            if (!erroNaoSequencial && missaoObject.CheckValidade()) {
                missaoObject.TimelineToMissao();
                
            }
        }

        // Display last time the timeline was saved
        if (missaoObject.lastSaveTime != null) {
            GUILayout.Space(padding);
            EditorGUILayout.LabelField("Última vez salvo: " + missaoObject.lastSaveTime);
        }
    }

    TimelineElement Anterior(TimelineElement element) {
        int index = missaoObject.timeline.IndexOf(element);
        if (index > 0) {
            return missaoObject.timeline[index - 1];
        }
        return null;
    }

    TimelineElement Proximo(TimelineElement element) {
        int index = missaoObject.timeline.IndexOf(element);
        if (index < missaoObject.timeline.Count - 1) {
            return missaoObject.timeline[index + 1];
        }
        return null;
    }

    bool CanElementGoUp(TimelineElement element) {
        if (element.tipo == TimelineElementType.FimNaoSequencial) {
            TimelineElement anterior = Anterior(element);
            if (anterior != null && anterior.tipo == TimelineElementType.ComecoNaoSequencial) {
                return false;
            }
        }
        return true;
    }

    bool CanElementGoDown(TimelineElement element) {
        if (element.tipo == TimelineElementType.ComecoNaoSequencial && element.mostrandoFilhos) {
            TimelineElement proximo = Proximo(element);
            if (proximo != null && proximo.tipo == TimelineElementType.FimNaoSequencial) {
                return false;
            }
        }
        return true;
    }

    void DrawHeader(TimelineElement element, string nome) {
        GUILayout.BeginVertical("HelpBox");

        GUILayout.BeginHorizontal();

        if (element.tipo == TimelineElementType.ComecoNaoSequencial) {
            string character = element.mostrandoFilhos ? "▼" : "▶";
            if (GUILayout.Button(character, GUILayout.Width(20), GUILayout.Height(20))) {
                element.mostrandoFilhos = !element.mostrandoFilhos;
            }
        }

        GUILayout.Label(nome, EditorStyles.boldLabel);

        // Buttons to go up and down in the timeline
        if (CanElementGoUp(element) && GUILayout.Button("▲", GUILayout.Width(20), GUILayout.Height(20))) {
            int index = missaoObject.timeline.IndexOf(element);
            if (index > 0) {
                elementMoveUp.Add(element);

                if (element.tipo == TimelineElementType.ComecoNaoSequencial && !element.mostrandoFilhos) {
                    TimelineElement filho = Proximo(element);
                    while (filho != null && filho.tipo != TimelineElementType.FimNaoSequencial) {
                        elementMoveUp.Add(filho);
                        filho = Proximo(filho);
                    }

                    if (filho != null) {
                        elementMoveUp.Add(filho);
                    }
                }
            }
        }

        if (CanElementGoDown(element) && GUILayout.Button("▼", GUILayout.Width(20), GUILayout.Height(20))) {
            int index = missaoObject.timeline.IndexOf(element);
            if (index < missaoObject.timeline.Count - 1) {
                elementMoveDown.Add(element);

                if (element.tipo == TimelineElementType.ComecoNaoSequencial && !element.mostrandoFilhos && missaoObject.timeline.IndexOf(element.naoSequencial) < missaoObject.timeline.Count - 1) {
                    TimelineElement filho = Proximo(element);
                    while (filho != null && filho.tipo != TimelineElementType.FimNaoSequencial) {
                        elementMoveDown.Add(filho);
                        filho = Proximo(filho);
                    }

                    if (filho != null) {
                        elementMoveDown.Add(filho);
                    }
                }
            }
        }

        if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) {
            if (element.tipo == TimelineElementType.ComecoNaoSequencial && Proximo(element) != null && Proximo(element).tipo != TimelineElementType.FimNaoSequencial) {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Remover somente si mesmo"), false, () => {
                    elementRemove.Add(element);
                    elementRemove.Add(element.naoSequencial);
                });
                menu.AddItem(new GUIContent("Remover somente filhos"), false, () => {
                    TimelineElement filho = Proximo(element);
                    while (filho != null && filho.tipo != TimelineElementType.FimNaoSequencial) {
                        elementRemove.Add(filho);
                        filho = Proximo(filho);
                    }
                });
                menu.AddItem(new GUIContent("Remover a si e seus filhos"), false, () => {
                    elementRemove.Add(element);

                    TimelineElement filho = Proximo(element);
                    while (filho != null && filho.tipo != TimelineElementType.FimNaoSequencial) {
                        elementRemove.Add(filho);
                        filho = Proximo(filho);
                    }
                    
                    if (filho != null) {
                        elementRemove.Add(filho);
                    }
                });
                menu.ShowAsContext();
            } else {
                elementRemove.Add(element);
                if (element.tipo == TimelineElementType.ComecoNaoSequencial || element.tipo == TimelineElementType.FimNaoSequencial) {
                    elementRemove.Add(element.naoSequencial);
                }
            }
        }

        GUILayout.EndHorizontal();
    }

    void DrawFooter(TimelineElement element) {
        GUILayout.EndVertical();
    }

    string DrawEnderecoField(string endereco, string id) {
        string enderecoNovo = EditorGUILayout.TextField(endereco);

        if (GUILayout.Button("...", GUILayout.Width(30), GUILayout.Height(20))) {
            EditorGUIUtility.ShowObjectPicker<GameObject>(null, true, "t:endereco", 0);
            chamouPicker = id;
        }

        if( Event.current.commandName == "ObjectSelectorUpdated" && chamouPicker == id)  {
            chamouPicker = "";
            GameObject selecionado = (GameObject)EditorGUIUtility.GetObjectPickerObject();
            if (selecionado.GetComponent<Endereco>() != null) {
                enderecoNovo = selecionado.GetComponent<Endereco>().nome;
            }
        }

        return enderecoNovo;
    }


    // Chamada por DrawObjetivo
    void DrawCarga(List<CargaObject> cargas, int i, List<CargaObject> cargasRemover, int conjuntoId) {
        Color oldColor = GUI.backgroundColor;
        if (oldColor != Color.red) {
            GUI.backgroundColor = Color.magenta;
        }

        int paddingCarga = 2;
        GUILayout.BeginVertical("HelpBox");
        GUILayout.Space(paddingCarga);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Carga " + i, EditorStyles.boldLabel);
        if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) {
            cargasRemover.Add(cargas[i]);
        }
        EditorGUILayout.EndHorizontal();

        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Peso:");
        cargas[i].peso = EditorGUILayout.FloatField(cargas[i].peso);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Fragilidade:");
        cargas[i].fragilidade = EditorGUILayout.FloatField(cargas[i].fragilidade);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Destinatario:");
        cargas[i].destinatario = DrawEnderecoField(cargas[i].destinatario, "Carga" + conjuntoId + "_" + i);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tipo:");
        cargas[i].tipo = (TipoCarga)EditorGUILayout.EnumPopup(cargas[i].tipo);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(paddingCarga);
        GUILayout.EndVertical();
        GUILayout.Space(paddingCarga/2);

        if (GUI.backgroundColor == Color.magenta) {
            GUI.backgroundColor = oldColor;
        }

    }

    void DrawObjetivo(TimelineElement objetivo, int id) {
        if (objetivo.objetivo == null) return;

        int paddingCarga = 2;

        DrawHeader(objetivo, "Objetivo");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Endereço:");
        //objetivo.objetivo.endereco = EditorGUILayout.TextField(objetivo.objetivo.endereco);
        objetivo.objetivo.endereco = DrawEnderecoField(objetivo.objetivo.endereco, "Objetivo" + id);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Permite receber:");
        objetivo.objetivo.permiteReceber = EditorGUILayout.Toggle(objetivo.objetivo.permiteReceber);
        EditorGUILayout.EndHorizontal();

        List<CargaObject> cargas = objetivo.objetivo.cargas;
        if (cargas == null) cargas = new List<CargaObject>();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Cargas:");
        if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20))) {
            cargas.Add(new CargaObject());
        }

        if (cargas.Count > 0 && GUILayout.Button("...", GUILayout.Width(20), GUILayout.Height(20))) {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Gerar Sequencial"), false, () => {
                foreach (TimelineElement element in GenerateTimelineFromCargas(cargas)) {
                    missaoObject.timeline.Add(element);
                }
            });
            menu.AddItem(new GUIContent("Gerar Não Sequencial"), false, () => {
                TimelineElement naoSequencialInicio = new TimelineElement();
                naoSequencialInicio.tipo = TimelineElementType.ComecoNaoSequencial;

                TimelineElement naoSequencialFim = new TimelineElement();
                naoSequencialFim.tipo = TimelineElementType.FimNaoSequencial;

                naoSequencialInicio.naoSequencial = naoSequencialFim;
                naoSequencialFim.naoSequencial = naoSequencialInicio;
                

                missaoObject.timeline.Add(naoSequencialInicio);
                
                foreach (TimelineElement element in GenerateTimelineFromCargas(cargas)) {
                    missaoObject.timeline.Add(element);
                }

                missaoObject.timeline.Add(naoSequencialFim);
            });
            menu.ShowAsContext();
        }
        
        EditorGUILayout.EndHorizontal();

        GUILayout.BeginVertical("box");
        GUILayout.Space(paddingCarga);

        List<CargaObject> cargasRemover = new List<CargaObject>();

        for (int i = 0; i < cargas.Count; i++) {
            DrawCarga(cargas, i, cargasRemover, id);
        }

        foreach (CargaObject carga in cargasRemover) {
            cargas.Remove(carga);
        }


        GUILayout.Space(paddingCarga/2);
        GUILayout.EndVertical();

        DrawFooter(objetivo);
    }

    void DrawDiretriz(TimelineElement diretriz) {
        if (diretriz.diretriz == null) return;

        DrawHeader(diretriz, "Diretriz");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Texto:");
        diretriz.diretriz.texto = EditorGUILayout.TextField(diretriz.diretriz.texto);
        EditorGUILayout.EndHorizontal();

        DrawFooter(diretriz);
    }

    void DrawNaoSequencialInicio(TimelineElement naoSequencial) {
        if (GUI.backgroundColor == Color.cyan) {
            GUI.backgroundColor = Color.red;
            erroNaoSequencial = true;
        }
        if (GUI.backgroundColor == Color.white) {
            GUI.backgroundColor = Color.cyan;
            erroNaoSequencial = false;
        }
        DrawHeader(naoSequencial, "Inicio - Não Sequencial");
        DrawFooter(naoSequencial);
        
    }

    void DrawNaoSequencialFim(TimelineElement naoSequencial) {
        DrawHeader(naoSequencial, "Fim - Não Sequencial");
        DrawFooter(naoSequencial);
        if (GUI.backgroundColor == Color.cyan) GUI.backgroundColor = Color.white;
        if (GUI.backgroundColor == Color.red) GUI.backgroundColor = Color.cyan;
    }

    void AddObjetivo() {
        TimelineElement objetivo = new TimelineElement();
        objetivo.tipo = TimelineElementType.Objetivo;

        missaoObject.timeline.Add(objetivo);
    }

    TimelineElement[] GenerateTimelineFromCargas(List<CargaObject> cargas) {
        List<string> enderecos = new List<string>();

        foreach (CargaObject carga in cargas) {
            if (!enderecos.Contains(carga.destinatario)) enderecos.Add(carga.destinatario);
        }

        TimelineElement[] timeline = new TimelineElement[enderecos.Count];
        for (int i = 0; i < enderecos.Count; i++) {
            TimelineElement element = new TimelineElement();
            element.tipo = TimelineElementType.Objetivo;
            element.objetivo = new ObjetivoObject();
            element.objetivo.endereco = enderecos[i];
            element.objetivo.permiteReceber = true;
            timeline[i] = element;
        }

        return timeline;
    }

    void AddDiretriz() {
        TimelineElement diretriz = new TimelineElement();
        diretriz.tipo = TimelineElementType.Diretriz;

        missaoObject.timeline.Add(diretriz);
    }

    void AddNaoSequencial() {
        TimelineElement naoSequencialInicio = new TimelineElement();
        naoSequencialInicio.tipo = TimelineElementType.ComecoNaoSequencial;

        TimelineElement naoSequencialFim = new TimelineElement();
        naoSequencialFim.tipo = TimelineElementType.FimNaoSequencial;

        naoSequencialInicio.naoSequencial = naoSequencialFim;
        naoSequencialFim.naoSequencial = naoSequencialInicio;
        

        missaoObject.timeline.Add(naoSequencialInicio);
        missaoObject.timeline.Add(naoSequencialFim);
    }
}

#endif