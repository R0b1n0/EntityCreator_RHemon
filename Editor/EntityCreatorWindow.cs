using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EntityGeneratorTool
{
    public class EntityCreatorWindow : EditorWindow
    {
        SerializedObject m_entityWrapper;
        SoEntity m_instance;
        Type m_selectedType;
        List<Type> m_subClass;
        List<string> m_subclassNames;

        int m_selectedTypeIndex;

        [MenuItem("EntityGenerator/EntGenWindow")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<EntityCreatorWindow>("EntityEditor");
        }

        private void OnEnable()
        {
            GetAllSubClasses();
            m_selectedTypeIndex = 0;
            DisplaySelectedClass();
        }

        private void OnGUI()
        {
            TypeSelection();
            if (m_entityWrapper != null)
            {
                GUILayout.Space(5);
                GUILayout.Label($"{m_entityWrapper.targetObject.GetType().Name} Editor");

                if (GUILayout.Button("Create ScriptableObject"))
                {
                    SaveScriptableObject();
                }
                if (m_entityWrapper != null)
                {
                    DisplayFields();
                }

            }
        }

        #region Type Selection
        private void TypeSelection()
        {
            int newValue = EditorGUILayout.Popup("SubClass Selection", m_selectedTypeIndex, m_subclassNames.ToArray());

            if (m_selectedTypeIndex != newValue)
            {
                m_selectedTypeIndex = newValue;
                DisplaySelectedClass();
            }
        }

        private void DisplaySelectedClass()
        {
            m_instance = (SoEntity)ScriptableObject.CreateInstance(m_subClass[m_selectedTypeIndex]);
            m_entityWrapper = new SerializedObject(m_instance);
        }

        private void GetAllSubClasses()
        {
            m_subClass = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(SoEntity)))).ToList();
            m_subclassNames = new List<string>();
            foreach(var obj in m_subClass)
            {
                m_subclassNames.Add(obj.Name);
            }
        }

        #endregion

        #region Display
        private void DisplayFields()
        {
            DisplayPrivateFields();
            m_entityWrapper.Update();
            var property = m_entityWrapper.GetIterator();
            property.NextVisible(true);
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Editable Fields :");

            while (property.NextVisible(false))
            {
                EditorGUILayout.PropertyField(property, new GUIContent(property.displayName), true);
            }

            m_entityWrapper.ApplyModifiedProperties();
        }
        private void DisplayPrivateFields()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Private Fields :");
            foreach (FieldInfo field in m_instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                EditorGUILayout.LabelField(field.Name);
            }
        }
        #endregion

        private void SaveScriptableObject()
        {
            if (m_entityWrapper == null)
            {
                Debug.LogError("No ScriptableObject instance to save!");
                return;
            }

            string path = EditorUtility.SaveFilePanelInProject(
                "Save ScriptableObject",
                m_instance.myName,
                "asset",
                "Select a location to save the ScriptableObject"
            );

            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(m_entityWrapper.targetObject, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Selection.activeObject = m_entityWrapper.targetObject;

                m_entityWrapper = null;
            }
        }
    }
}

