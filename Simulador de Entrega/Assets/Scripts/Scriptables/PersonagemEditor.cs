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

        if (personagem.portrait != null) {
            Texture2D texture = AssetPreview.GetAssetPreview(personagem.portrait);
            if (texture != null) {
                texture.filterMode = FilterMode.Point;
                GUILayout.Label("", GUILayout.Height(240), GUILayout.Width(240));
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }
        }

        if (personagem.portraitGrande != null) {
            Texture2D texture = AssetPreview.GetAssetPreview(personagem.portraitGrande);
            if (texture != null) {
                texture.filterMode = FilterMode.Point;
                GUILayout.Label("", GUILayout.Height(480), GUILayout.Width(480));
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }
        }
        
        
    }
}

#endif