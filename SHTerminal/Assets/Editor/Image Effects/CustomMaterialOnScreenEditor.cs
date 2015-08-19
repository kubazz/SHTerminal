
using UnityEditor;
using System.Collections;
using UnityEngine;

[CustomEditor(typeof(CustomMaterialOnScreen)), CanEditMultipleObjects] 

public class CustomMaterialOnScreenEditor : Editor {	
	
    public override void OnInspectorGUI() {

        var CustomMaterialOnScreen = (CustomMaterialOnScreen)target;

        CustomMaterialOnScreen.Name = EditorGUILayout.TextField("Name",CustomMaterialOnScreen.Name);

        EditorGUILayout.BeginHorizontal();
        Shader prevShader = CustomMaterialOnScreen.Shader;        
        CustomMaterialOnScreen.Shader = (Shader)EditorGUILayout.ObjectField("Shader", CustomMaterialOnScreen.Shader, typeof(Shader), true, null);
        
        if (CustomMaterialOnScreen.Shader != prevShader || GUILayout.Button("Reload"))
        {
            CustomMaterialOnScreen.SetupMaterialInfo();
        }
        EditorGUILayout.EndHorizontal();

        foreach (var materialProperty in CustomMaterialOnScreen.MaterialProperties)
        {
            materialProperty.Get().DrawInspector();
        }

        EditorUtility.SetDirty(target);
    }
}