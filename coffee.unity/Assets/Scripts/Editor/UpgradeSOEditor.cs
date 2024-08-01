using System;
using System.Linq;
using System.Reflection;
using POLYGONWARE.Coffee.Player;
using UnityEditor;

namespace POLYGONWARE.Coffee.Editor
{
    [CustomEditor(typeof(UpgradeSO))]
    public class UpgradeSOEditor : UnityEditor.Editor
    {
        private SerializedProperty _icon;
        private SerializedProperty _name;
        private SerializedProperty _description;
        private SerializedProperty _costString;
        private SerializedProperty _cooldown;
        private SerializedProperty _value;
        private SerializedProperty _timeToUpgrade;
        private SerializedProperty _unlockLevel;
        private SerializedProperty _typeName;
        private SerializedProperty _soundClue1;
        private SerializedProperty _prefab;

        private string[] _upgradeTypes;
        private int _selectedTypeIndex;

        private void OnEnable()
        {
            _icon = serializedObject.FindProperty("_icon");
            _name = serializedObject.FindProperty("_name");
            _description = serializedObject.FindProperty("_description");
            _costString = serializedObject.FindProperty("_costString");
            _cooldown = serializedObject.FindProperty("_cooldown");
            _value = serializedObject.FindProperty("_value");
            _timeToUpgrade = serializedObject.FindProperty("_timeToUpgrade");
            _unlockLevel = serializedObject.FindProperty("_unlockLevel");
            _typeName = serializedObject.FindProperty("_typeName");
            _soundClue1 = serializedObject.FindProperty("_soundClue1");
            _prefab = serializedObject.FindProperty("_prefab");

            // Find all types that inherit from Upgrade
            _upgradeTypes = Assembly.GetAssembly(typeof(Upgrade))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Upgrade)) && !t.IsAbstract)
                .Select(t => t.FullName)
                .ToArray();

            // Get the currently selected type
            _selectedTypeIndex = Array.IndexOf(_upgradeTypes, _typeName.stringValue);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_icon);
            EditorGUILayout.PropertyField(_name);
            EditorGUILayout.PropertyField(_description);
            EditorGUILayout.PropertyField(_costString);
            EditorGUILayout.PropertyField(_cooldown);
            EditorGUILayout.PropertyField(_value);
            EditorGUILayout.PropertyField(_timeToUpgrade);
            EditorGUILayout.PropertyField(_unlockLevel);
            EditorGUILayout.PropertyField(_prefab);
            
            // Display the dropdown for selecting the upgrade type
            var ugpradeTypesTrimed = _upgradeTypes.Select(t => t.Substring(t.LastIndexOf('.') + 1)).ToArray();
            _selectedTypeIndex = EditorGUILayout.Popup("Upgrade Type", _selectedTypeIndex, ugpradeTypesTrimed);
            if (_selectedTypeIndex >= 0 && _selectedTypeIndex < _upgradeTypes.Length)
            {
                _typeName.stringValue = _upgradeTypes[_selectedTypeIndex];
            }

            EditorGUILayout.PropertyField(_soundClue1);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
