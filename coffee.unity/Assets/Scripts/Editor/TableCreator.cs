using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace POLYGONWARE.Coffee.Editor
{
    public class TableCreator : EditorWindow
    {
        private Vector2 _scrollPosition;
        private FieldInfo[] _fields;
        private Type _structType;
        private IList _dataList;
        private object _structObject;
        private bool _showSpriteInField = false;
        private object _pickerItem; // Item currently being edited
        private FieldInfo _pickerField; // Field currently being edited
        private List<Type> _structTypes;
        private int _selectedStructIndex = 0;
        private string _searchString = string.Empty;
        private List<Type> _filteredStructTypes;

        [MenuItem("Window/Data/Data Table Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<TableCreator>();
            window.titleContent = new GUIContent("Data Table Editor");
            window.Show();
        }

        private void OnEnable()
        {
            _structTypes = GetStructTypes();
            _filteredStructTypes = new List<Type>(_structTypes);
            if (_structTypes.Any())
            {
                _structType = _structTypes[_selectedStructIndex];
                _fields = _structType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                _dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_structType));
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            _showSpriteInField = EditorGUILayout.Toggle("Show Sprites in Fields", _showSpriteInField);

            EditorGUILayout.LabelField("Search Struct Type");
            _searchString = EditorGUILayout.TextField(_searchString);
            if (GUILayout.Button("Search"))
            {
                SearchStructTypes();
            }

            _selectedStructIndex = EditorGUILayout.Popup("Select Struct Type", _selectedStructIndex, _filteredStructTypes.Select(t => t.Name).ToArray());
            if (_structType != _filteredStructTypes[_selectedStructIndex])
            {
                _structType = _filteredStructTypes[_selectedStructIndex];
                _fields = _structType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                _dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_structType));
            }

            EditorGUILayout.EndVertical();

            if (_structType == null)
            {
                EditorGUILayout.HelpBox("Please select a struct type", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space();

            DrawHeader();
            DrawRows();

            EditorGUILayout.Space();

            if (GUILayout.Button("Add New Row"))
            {
                AddNewRow();
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Export to CSV"))
            {
                ExportToCSV();
            }

            if (GUILayout.Button("Import from CSV"))
            {
                ImportFromCSV();
            }

            EditorGUILayout.EndHorizontal();
            HandlePickSprite();
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal();
            foreach (var field in _fields)
            {
                GUILayout.Label(field.Name, EditorStyles.boldLabel, GUILayout.Width(GetFieldWidth(field.FieldType)));
            }
            GUILayout.Label("Actions", EditorStyles.boldLabel, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawRows()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            int removeIndex = -1;

            for (int i = 0; i < _dataList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                var item = _dataList[i];
                foreach (var field in _fields)
                {
                    DrawField(ref item, field);
                }
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    removeIndex = i;
                }
                _dataList[i] = item;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            if (removeIndex >= 0)
            {
                _dataList.RemoveAt(removeIndex);
            }
        }

        private void DrawField(ref object item, FieldInfo field)
        {
            object value = field.GetValue(item);
            Type fieldType = field.FieldType;

            if (fieldType == typeof(int))
            {
                field.SetValue(item, EditorGUILayout.IntField((int)value, GUILayout.Width(GetFieldWidth(fieldType))));
            }
            else if (fieldType == typeof(float))
            {
                field.SetValue(item, EditorGUILayout.FloatField((float)value, GUILayout.Width(GetFieldWidth(fieldType))));
            }
            else if (fieldType == typeof(string))
            {
                field.SetValue(item, EditorGUILayout.TextField((string)value, GUILayout.Width(GetFieldWidth(fieldType))));
            }
            else if (fieldType == typeof(Sprite))
            {
                if (_showSpriteInField)
                {
                    var sprite = value as Sprite;
                    if (sprite != null)
                    {
                        Rect spriteRect = sprite.rect;
                        Rect texCoords = new Rect(
                            spriteRect.x / sprite.texture.width,
                            spriteRect.y / sprite.texture.height,
                            spriteRect.width / sprite.texture.width,
                            spriteRect.height / sprite.texture.height
                        );

                        if (GUILayout.Button("", GUILayout.Width(GetFieldWidth(fieldType)), GUILayout.Height(GetFieldWidth(fieldType))))
                        {
                            EditorGUIUtility.ShowObjectPicker<Sprite>(sprite, false, null, 0);
                            _pickerItem = item;
                            _pickerField = field;
                        }

                        Rect lastRect = GUILayoutUtility.GetLastRect();
                        GUI.DrawTextureWithTexCoords(lastRect, sprite.texture, texCoords);
                    }
                    else
                    {
                        field.SetValue(item, (Sprite)EditorGUILayout.ObjectField((Sprite)value, typeof(Sprite), allowSceneObjects: false, GUILayout.Width(GetFieldWidth(fieldType))));
                    }
                }
                else
                {
                    field.SetValue(item, (Sprite)EditorGUILayout.ObjectField((Sprite)value, typeof(Sprite), allowSceneObjects: false, GUILayout.Width(GetFieldWidth(fieldType))));
                }
            }
            else if (fieldType.IsEnum)
            {
                field.SetValue(item, EditorGUILayout.EnumPopup((Enum)value, GUILayout.Width(GetFieldWidth(fieldType))));
            }
            else
            {
                GUILayout.Label("Unsupported Type", GUILayout.Width(GetFieldWidth(fieldType)));
            }
        }

        private void AddNewRow()
        {
            var newItem = Activator.CreateInstance(_structType);
            _dataList.Add(newItem);
        }

        private float GetFieldWidth(Type fieldType)
        {
            if (fieldType == typeof(int) || fieldType == typeof(float) || fieldType == typeof(string))
            {
                return 150;
            }
            else if (fieldType == typeof(Sprite))
            {
                if (_showSpriteInField)
                {
                    return 100;
                }
                return 100;
            }
            else if (fieldType.IsEnum)
            {
                return 100;
            }
            else
            {
                return 100; // Default width for unsupported types
            }
        }

        private void ExportToCSV()
        {
            string path = EditorUtility.SaveFilePanel("Export CSV", "", "data.csv", "csv");
            if (string.IsNullOrEmpty(path)) return;

            using (StreamWriter writer = new StreamWriter(path))
            {
                // Write header
                writer.WriteLine(string.Join(",", _fields.Select(f => f.Name)));

                // Write data
                foreach (var item in _dataList)
                {
                    var values = _fields.Select(f =>
                    {
                        var value = f.GetValue(item);
                        if (f.FieldType == typeof(Sprite) && value != null)
                        {
                            var sprite = value as Sprite;
                            var texturePath = AssetDatabase.GetAssetPath(sprite.texture);
                            return $"{texturePath}:{sprite.name}";
                        }
                        return value?.ToString().Replace(",", ";");
                    }).ToArray();
                    writer.WriteLine(string.Join(",", values));
                }
            }

            AssetDatabase.Refresh();
        }

        private void ImportFromCSV()
        {
            string path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
            if (string.IsNullOrEmpty(path)) return;

            var newDataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_structType));

            using (StreamReader reader = new StreamReader(path))
            {
                // Read header
                var header = reader.ReadLine();
                if (header == null) return;

                var fieldNames = header.Split(',');

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var newItem = Activator.CreateInstance(_structType);
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        var field = _fields.FirstOrDefault(f => f.Name == fieldNames[i]);
                        if (field == null) continue;

                        object value = ConvertValue(field.FieldType, values[i].Replace(";", ","));
                        field.SetValue(newItem, value);
                    }

                    newDataList.Add(newItem);
                }
            }

            _dataList.Clear();
            foreach (var item in newDataList)
            {
                _dataList.Add(item);
            }
        }

        private object ConvertValue(Type fieldType, string value)
        {
            if (fieldType == typeof(int))
            {
                return int.Parse(value);
            }
            else if (fieldType == typeof(float))
            {
                return float.Parse(value);
            }
            else if (fieldType == typeof(string))
            {
                return value;
            }
            else if (fieldType.IsEnum)
            {
                return Enum.Parse(fieldType, value);
            }
            else if (fieldType == typeof(Sprite))
            {
                var parts = value.Split(':');
                if (parts.Length == 2)
                {
                    var texturePath = parts[0];
                    var spriteName = parts[1];
                    var sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>();
                    return sprites.FirstOrDefault(s => s.name == spriteName);
                }
            }
            // Add more field types as needed
            return null;
        }

        private void HandlePickSprite()
        {
            Event e = Event.current;
            if (e.commandName == "ObjectSelectorUpdated")
            {
                if (EditorGUIUtility.GetObjectPickerObject() is not Sprite sprite)
                {
                    return;
                }

                Sprite selectedSprite = (Sprite)EditorGUIUtility.GetObjectPickerObject();
                if (selectedSprite != null)
                {
                    Debug.Log("Selected sprite: " + selectedSprite.name);
                }
                else
                {
                    Debug.Log("No sprite selected");
                }

                if (_pickerItem != null && _pickerField != null)
                {
                    int index = _dataList.IndexOf(_pickerItem);
                    Debug.Log("index: " + index);
                    _pickerField.SetValue(_pickerItem, selectedSprite);
                    _dataList[index] = _pickerItem; // Update the item in the list
                    Debug.Log("Sprite field updated ! " + _pickerItem + " " + _pickerField + " " + selectedSprite);
                    Repaint(); // Refresh the editor window
                }
                else
                {
                    Debug.LogWarning("Picker item or field is null");
                }
            }
        }

        private void SearchStructTypes()
        {
            if (string.IsNullOrWhiteSpace(_searchString))
            {
                _filteredStructTypes = new List<Type>(_structTypes);
            }
            else
            {
                _filteredStructTypes = _structTypes
                    .Where(t => t.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            _selectedStructIndex = 0;
            if (_filteredStructTypes.Count > 0)
            {
                _structType = _filteredStructTypes[_selectedStructIndex];
                _fields = _structType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                _dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_structType));
            }
            else
            {
                _structType = null;
                _fields = null;
                _dataList = null;
            }
        }

        private List<Type> GetStructTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsValueType && !type.IsPrimitive && !type.IsEnum)
                .ToList();
        }
    }
}
