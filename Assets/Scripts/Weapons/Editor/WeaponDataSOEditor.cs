using System;
using System.Collections.Generic;
using System.Linq;
using Avocado.Weapons.Components;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script personaliza el Editor de Unity para el objeto WeaponDataSO (que probablemente es un 
ScriptableObject que almacena datos sobre armas). Añade funcionalidades útiles directamente al 
Inspector, como:
-Agregar dinámicamente componentes de datos (daño, knockback, etc.) al arma.
-Inicializar todos los ataques con el número correcto de entradas.
-Actualizar nombres de ataques o componentes para mantenerlos organizados.
-Detectar automáticamente nuevos tipos derivados de ComponentData después de la recompilación, 
usando reflexión.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    [CustomEditor(typeof(WeaponDataSO))]
    public class WeaponDataSOEditor : Editor
    {
        // Lista estática con todos los tipos de datos que heredan de ComponentData
        private static List<Type> dataCompTypes = new List<Type>();

        // Referencia al objeto editado
        private WeaponDataSO dataSO;

        // Flags para mostrar u ocultar secciones del inspector
        private bool showForceUpdateButtons;
        private bool showAddComponentButtons;

        // Se llama cuando se habilita el editor
        private void OnEnable()
        {
            dataSO = target as WeaponDataSO;
        }

        // Personaliza el GUI del inspector
        public override void OnInspectorGUI()
        {
            // Dibuja el inspector predeterminado
            base.OnInspectorGUI();

            // Botón para inicializar el número de ataques en todos los componentes
            if (GUILayout.Button("Set Number of Attacks"))
            {
                foreach (var item in dataSO.ComponentData)
                {
                    item.InitializeAttackData(dataSO.NumberOfAttacks);
                }
            }

            // Sección plegable para agregar nuevos componentes
            showAddComponentButtons = EditorGUILayout.Foldout(showAddComponentButtons, "Add Components");

            if (showAddComponentButtons)
            {
                foreach (var dataCompType in dataCompTypes)
                {
                    // Botón para cada tipo de componente
                    if (GUILayout.Button(dataCompType.Name))
                    {
                        // Crea una instancia del tipo
                        var comp = Activator.CreateInstance(dataCompType) as ComponentData;

                        if (comp == null)
                            return;

                        // Inicializa y agrega al WeaponDataSO
                        comp.InitializeAttackData(dataSO.NumberOfAttacks);
                        dataSO.AddData(comp);

                        // Marca el objeto como modificado para guardar
                        EditorUtility.SetDirty(dataSO);
                    }
                }
            }

            // Sección plegable para forzar actualizaciones
            showForceUpdateButtons = EditorGUILayout.Foldout(showForceUpdateButtons, "Force Update Buttons");

            if (showForceUpdateButtons)
            {
                // Botón para actualizar los nombres de los componentes
                if (GUILayout.Button("Force Update Component Names"))
                {
                    foreach (var item in dataSO.ComponentData)
                    {
                        item.SetComponentName();
                    }
                }

                // Botón para actualizar los nombres de los ataques
                if (GUILayout.Button("Force Update Attack Names"))
                {
                    foreach (var item in dataSO.ComponentData)
                    {
                        item.SetAttackDataNames();
                    }
                }
            }
        }

        // Se ejecuta automáticamente cuando se recompilan los scripts
        [DidReloadScripts]
        private static void OnRecompile()
        {
            // Obtiene todos los tipos en todos los ensamblados
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(assembly => assembly.GetTypes());

            // Filtra solo los tipos válidos que heredan de ComponentData
            var filteredTypes = types.Where(type => type.IsSubclassOf(typeof(ComponentData)) && !type.ContainsGenericParameters && type.IsClass);

            // Guarda los tipos encontrados
            dataCompTypes = filteredTypes.ToList();
        }
    }
}
