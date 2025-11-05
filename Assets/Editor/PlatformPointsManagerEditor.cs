using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Avocado
{
    [CustomEditor(typeof(PlatformPointsManager))]
    public class PlatformPointsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Dibuja el Inspector por defecto
            DrawDefaultInspector();

            // Línea separadora
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(" GENERADOR DE PUNTOS", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Botones
            PlatformPointsManager manager = (PlatformPointsManager)target;

            if (GUILayout.Button($"Generar {manager.numPoints} Puntos", GUILayout.Height(30)))
            {
                manager.GeneratePoints();
                EditorUtility.SetDirty(manager);  // Marca como "sucio" para guardar cambios
                SceneView.RepaintAll();  // Refresca la SceneView
            }

            if (GUILayout.Button(" Borrar Puntos", GUILayout.Height(25)))
            {
                manager.ClearPoints();
                EditorUtility.SetDirty(manager);
                SceneView.RepaintAll();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Puntos actuales: {manager.points.Count}", EditorStyles.miniLabel);
        }
    }
}
