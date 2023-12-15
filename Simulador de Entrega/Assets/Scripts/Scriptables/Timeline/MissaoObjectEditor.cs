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

    GUIStyle textAreaStyle;

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
        
        textAreaStyle = new GUIStyle(EditorStyles.textArea);
        textAreaStyle.wordWrap = true;

        EditorGUILayout.LabelField("Descrição breve:");
        missaoObject.descricao = EditorGUILayout.TextArea(missaoObject.descricao, textAreaStyle, GUILayout.Height(60));

        EditorGUILayout.LabelField("Descrição longa:");
        missaoObject.descricaoGrande = EditorGUILayout.TextArea(missaoObject.descricaoGrande, textAreaStyle, GUILayout.Height(80));

        missaoObject.personagem = EditorGUILayout.ObjectField("Personagem:", missaoObject.personagem, typeof(PersonagemObject), false) as PersonagemObject;
        missaoObject.dialogo = EditorGUILayout.ObjectField("Dialogo:", missaoObject.dialogo, typeof(DialogoPersonagens), false) as DialogoPersonagens;

        GUILayout.Space(padding);

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
                    DrawDiretriz(element, counter);
                    break;
                case TimelineElementType.ComecoNaoSequencial:
                    DrawNaoSequencialInicio(element);
                    comecoNaoSequencial.Enqueue(element);
                    mostrar = element.mostrandoFilhos;
                    break;
                case TimelineElementType.FimNaoSequencial:
                    DrawNaoSequencialFim(element);
                    break;
                case TimelineElementType.Cutscene:
                    DrawCutscene(element);
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
            menu.AddItem(new GUIContent("Cutscene"), false, AddCutscene);
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

        GUILayout.Space(padding);
        GUILayout.Space(padding);
        SerializedProperty m_missoesDesbloqueadas = serializedObject.FindProperty("missoesDesbloqueadas");
        EditorGUILayout.PropertyField(m_missoesDesbloqueadas, new GUIContent("Desbloquear missões ao finalizar"), true);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Gerar missão aleatória no final:");
        missaoObject.gerarAleatoriaNoFinal = EditorGUILayout.Toggle(missaoObject.gerarAleatoriaNoFinal);
        EditorGUILayout.EndHorizontal();

        

        serializedObject.ApplyModifiedProperties();

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

        /*
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Peso:");
        cargas[i].peso = EditorGUILayout.FloatField(cargas[i].peso);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Fragilidade:");
        cargas[i].fragilidade = EditorGUILayout.FloatField(cargas[i].fragilidade);
        EditorGUILayout.EndHorizontal();
        */
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Destinatario:");
        cargas[i].destinatario = DrawEnderecoField(cargas[i].destinatario, "Carga" + conjuntoId + "_" + i);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Prefab:");
        cargas[i].prefab = (GameObject)EditorGUILayout.ObjectField(cargas[i].prefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();
/*
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tipo:");
        cargas[i].tipo = (TipoCarga)EditorGUILayout.EnumPopup(cargas[i].tipo);
        EditorGUILayout.EndHorizontal();
*/
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

        if (objetivo.objetivo.cargas == null) objetivo.objetivo.cargas = new List<CargaObject>();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Cargas:");
        if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20))) {
            objetivo.objetivo.cargas.Add(new CargaObject());
        }

        List<CargaObject> cargas = objetivo.objetivo.cargas;

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

    void DrawTimer(List<TimerObject> limites, int i, List<TimerObject> remover, int conjuntoId) {
        Color oldColor = GUI.backgroundColor;
        if (oldColor != Color.red) {
            GUI.backgroundColor = Color.magenta;
        }

        int halfPadding = padding/2;


        GUILayout.BeginVertical("HelpBox");
        GUILayout.Space(halfPadding);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Limite " + i, EditorStyles.boldLabel);
        if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) {
            remover.Add(limites[i]);
        }
        EditorGUILayout.EndHorizontal();

        TimerObject timer = limites[i];

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Tempo:");
        timer.tempo = EditorGUILayout.FloatField(timer.tempo);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(halfPadding/2);
        GUILayout.EndVertical();
        GUILayout.Space(halfPadding/2);

        if (GUI.backgroundColor == Color.magenta) {
            GUI.backgroundColor = oldColor;
        }
    }

    void DrawDiretriz(TimelineElement diretriz, int id) {
        if (diretriz.diretriz == null) return;
        int halfPadding = padding/2;


        DrawHeader(diretriz, "Diretriz");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Texto:");
        diretriz.diretriz.texto = EditorGUILayout.TextField(diretriz.diretriz.texto);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Limitações:");

        if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20))) {
            if (diretriz.diretriz.limitacoes == null) diretriz.diretriz.limitacoes = new List<TimerObject>();
            diretriz.diretriz.limitacoes.Add(new TimerObject());
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.BeginVertical("box");
        GUILayout.Space(halfPadding);

        List<TimerObject> limites = diretriz.diretriz.limitacoes;
        List<TimerObject> remover = new List<TimerObject>();

        if (limites == null) limites = new List<TimerObject>();

        for (int i = 0; i < limites.Count; i++) {
            DrawTimer(limites, i, remover, id);
        }

        foreach (TimerObject el in remover) {
            limites.Remove(el);
        }


        GUILayout.Space(halfPadding/2);
        GUILayout.EndVertical();

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

    void DrawCutscene(TimelineElement cutscene) {
        if (cutscene.cutscenes == null) return;

        int paddingCut = 2;

        DrawHeader(cutscene, "Cutscene");


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Cutscenes:");
        if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20))) {
            cutscene.cutscenes.Add(new CutsceneObject());
        }

        List<CutsceneObject> cutList = cutscene.cutscenes;

        EditorGUILayout.EndHorizontal();

        GUILayout.BeginVertical("box");
        GUILayout.Space(paddingCut);

        List<CutsceneObject> cutRemover = new List<CutsceneObject>();

        for (int i = 0; i < cutList.Count; i++) {
            Color oldColor = GUI.backgroundColor;
            if (oldColor != Color.red) {
                GUI.backgroundColor = Color.green;
            }
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Space(paddingCut);
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Cutscene " + i, EditorStyles.boldLabel);
            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) {
                cutRemover.Add(cutList[i]);
            }
            EditorGUILayout.EndHorizontal();

            FalaPersonagens fala = (cutList[i].fala==null) ? new FalaPersonagens() : cutList[i].fala;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Texto:");
            cutList[i].fala.fala = EditorGUILayout.TextField(cutList[i].fala.fala);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Audio:");
            cutList[i].fala.audio = (AudioClip)EditorGUILayout.ObjectField(cutList[i].fala.audio, typeof(AudioClip), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Animacao:");
            cutList[i].fala.animacao = (AnimacoesFala)EditorGUILayout.EnumPopup(cutList[i].fala.animacao);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Personagem:");
            cutList[i].personagem = (PersonagemObject)EditorGUILayout.ObjectField(cutList[i].personagem, typeof(PersonagemObject), false);


            GUILayout.Space(paddingCut);
            GUILayout.EndVertical();
            GUILayout.Space(paddingCut/2);

            if (GUI.backgroundColor == Color.green) {
                GUI.backgroundColor = oldColor;
            }
        }

        foreach (CutsceneObject cut in cutRemover) {
            cutList.Remove(cut);
        }


        GUILayout.Space(paddingCut/2);
        GUILayout.EndVertical();

        DrawFooter(cutscene);
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

    void AddCutscene() {
        TimelineElement cutscene = new TimelineElement();
        cutscene.tipo = TimelineElementType.Cutscene;
        cutscene.cutscenes = new List<CutsceneObject>();

        missaoObject.timeline.Add(cutscene);
    }
}

#endif