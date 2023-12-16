#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(AcessorioUpgrade))]
public class EditorAcessorioUpgrade : Editor {
    AcessorioUpgrade acessorio;
    bool mostrarFaltantes = false;

    void OnEnable() {
        acessorio = (AcessorioUpgrade)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (acessorio.acessorioPrefab == null) return;


        GUILayout.Label("Esse é o item, não o ícone:", EditorStyles.boldLabel);
        Texture2D texture = AssetPreview.GetAssetPreview(acessorio.acessorioPrefab);
        if (texture != null) {
             GUILayout.Label("", GUILayout.Height(120), GUILayout.Width(120));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        }

        if (acessorio.icone != null) {
            GUILayout.Label("Esse é o ícone:", EditorStyles.boldLabel);
            GUILayout.Label("", GUILayout.Height(120), GUILayout.Width(120));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), acessorio.icone);
        }

        if (GUILayout.Button("Mostrar faltantes?")) {
            mostrarFaltantes = !mostrarFaltantes;
        }

        if (mostrarFaltantes)
            GerarFaltantes();
    }

    public void GerarFaltantes() {
        string[] files = Directory.GetFiles("Assets/Prefabs/RoupinhaCarro/", "*.prefab", SearchOption.TopDirectoryOnly);

        GUILayout.Label("Olá se você não sabe o que é isso, não mexe nisso, obrigado!");

        GUILayout.Label("Faltantes", EditorStyles.boldLabel);

        GUILayout.BeginVertical("box");

        foreach (string file in files) {
            string currentFolder = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (Path.GetExtension(currentFolder) != "") currentFolder = currentFolder.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");

            // check if already exists
            string nome = Path.GetFileNameWithoutExtension(file);
            string scriptablePath = Path.Combine(currentFolder, nome + "Upgrade.asset");

            if (File.Exists(scriptablePath)) continue;

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);
            if (prefab == null) continue;

            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            if (texture == null) continue;

            GUILayout.BeginHorizontal();
            GUILayout.Button("", GUILayout.Height(120), GUILayout.Width(120));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            GUILayout.Label(Path.GetFileNameWithoutExtension(file));

            if (GUILayout.Button("Gerar")) {
                // Cria um novo scriptable object
                AcessorioUpgrade novo = ScriptableObject.CreateInstance<AcessorioUpgrade>();
                novo.acessorioPrefab = prefab;
                novo.nome = nome;
                
                // Salva o scriptable object
                AssetDatabase.CreateAsset(novo, scriptablePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                // Seleciona o novo scriptable object
                Selection.activeObject = novo;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
}

#endif