#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PersonagemObject))]
public class PersonagemEditor : Editor {
    PersonagemObject personagem;

    void OnEnable() {
        personagem = (PersonagemObject)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (personagem.portrait == null) return;

        Texture2D texture = AssetPreview.GetAssetPreview(personagem.portrait);
        if (texture == null) return;
        
        texture.filterMode = FilterMode.Point;
        GUILayout.Label("", GUILayout.Height(240), GUILayout.Width(240));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}

#endif