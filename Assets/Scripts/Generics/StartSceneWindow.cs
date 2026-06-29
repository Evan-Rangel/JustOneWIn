using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StartSceneWindow : EditorWindow
{
    [MenuItem("Tools/Play Mode Scene Selector")]
    public static void ShowWindow()
    {
        GetWindow<StartSceneWindow>("Play Mode Scene");
    }

    private void OnGUI()
    {
        EditorSceneManager.playModeStartScene = (SceneAsset)EditorGUILayout.ObjectField(
            new GUIContent("Escena de Inicio:"),
            EditorSceneManager.playModeStartScene,
            typeof(SceneAsset),
            false
        );

        if (GUILayout.Button("Limpiar escena asignada"))
        {
            EditorSceneManager.playModeStartScene = null;
        }
    }
}
