﻿using MultiMod.Shared;
using UnityEditor;
using UnityEngine;

namespace MultiMod.Editor
{
    [CustomEditor(typeof(ExportSettings))]
    public class ExportSettingsEditor : UnityEditor.Editor
    {
        private SerializedProperty _author;
        private SerializedProperty _description;
        private SerializedProperty _name;
        private SerializedProperty _outputDirectory;
        private SerializedProperty _version;
        private SerializedProperty _prefab;

        private void OnEnable()
        {
            _name = serializedObject.FindProperty("_name");
            _author = serializedObject.FindProperty("_author");
            _description = serializedObject.FindProperty("_description");
            _version = serializedObject.FindProperty("_version");
            _outputDirectory = serializedObject.FindProperty("_outputDirectory");
            _prefab = serializedObject.FindProperty("_prefab");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(5);

            EditorGUILayout.PropertyField(_name, new GUIContent("Mod Name*:"));
            EditorGUILayout.PropertyField(_author, new GUIContent("Author:"));
            EditorGUILayout.PropertyField(_version, new GUIContent("Version:"));
            EditorGUILayout.PropertyField(_prefab, new GUIContent("Startup Prefab:"));

            EditorGUILayout.PropertyField(_description, new GUIContent("Description:"), GUILayout.Height(60));

            GUILayout.Space(5);

            LogUtility.logLevel = (LogLevel) EditorGUILayout.EnumPopup("Log Level:", LogUtility.logLevel);

            var enabled = GUI.enabled;

            GUILayout.BeginHorizontal();

            GUI.enabled = false;

            EditorGUILayout.TextField("Output Directory*:", GetShortString(_outputDirectory.stringValue));

            GUI.enabled = enabled;

            if (GUILayout.Button("...", GUILayout.Width(30)))
            {
                var selectedDirectory =
                    EditorUtility.SaveFolderPanel("Choose output directory", _outputDirectory.stringValue, "");
                if (!string.IsNullOrEmpty(selectedDirectory))
                    _outputDirectory.stringValue = selectedDirectory;

                Repaint();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private string GetShortString(string str)
        {
            var maxWidth = (int) EditorGUIUtility.currentViewWidth - 252;
            var cutoffIndex = Mathf.Max(0, str.Length - 7 - maxWidth / 7);
            var shortString = str.Substring(cutoffIndex);
            if (cutoffIndex > 0)
                shortString = "..." + shortString;
            return shortString;
        }
    }
}