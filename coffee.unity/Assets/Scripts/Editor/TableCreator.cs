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
        private Type _selectedType;
        private IList _dataList;
        private List<string> _uniqueIdentifiers = new();
        private object _selectedObject;
        private bool _showSpriteInField = false;
        private bool _uniqueIdentifier = false; // extra first column for unique identifier
        private bool _customUniqueIdentifier = false; // if false, index is used as unique identifier
        private string _namePrefix = string.Empty;
        private string _nameSuffix = string.Empty;
        
        private object _pickerItem; // Item currently being edited
        private FieldInfo _pickerField; // Field currently being edited
        private List<Type> _structTypes;
        private List<Type> _scriptableObjectTypes;
        private int _selectedTypeIndex = 0;
        private string _searchString = string.Empty;
        private List<Type> _filteredTypes;
        //private bool _isStructSelected = true;
        private SupportedType _selectedSupportedType = SupportedType.STRUCT;
        
        private Dictionary<string, bool> _fieldCheckboxes = new();

        private enum SupportedType
        {
            STRUCT,
            SCRIPTABLE_OBJECT
        }

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
            _scriptableObjectTypes = GetScriptableObjectTypes();
            _filteredTypes = new List<Type>(_structTypes);
            if (_filteredTypes.Any())
            {
                _selectedType = _filteredTypes[_selectedTypeIndex];
                InitializeFields();
                
                // _fields = _selectedType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                // _dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_selectedType));
                // _uniqueIdentifiers = new List<string>();
                //
                // // init checkboxes
                // _fieldCheckboxes = _fields.ToDictionary(f => f.Name, f => false);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(500));
            _showSpriteInField = EditorGUILayout.Toggle("Show Sprites in Fields", _showSpriteInField);
            _uniqueIdentifier = EditorGUILayout.Toggle("Unique Identifier", _uniqueIdentifier);

            if (_uniqueIdentifier)
            {
                _customUniqueIdentifier = EditorGUILayout.Toggle("Custom Unique Identifier", _customUniqueIdentifier);
            }

            // _isStructSelected = EditorGUILayout.Toggle("Struct", _isStructSelected);
            _selectedSupportedType = (SupportedType)EditorGUILayout.EnumPopup("Select Type", _selectedSupportedType);

            if (_selectedSupportedType == SupportedType.SCRIPTABLE_OBJECT)
            {
                // settings for scriptable objects
                EditorGUILayout.LabelField("ScriptableObject Settings");
                _namePrefix = EditorGUILayout.TextField("Export Prefix", _namePrefix);
                _nameSuffix = EditorGUILayout.TextField("Export Suffix", _nameSuffix);
            }

            EditorGUILayout.LabelField("Search Type");
            _searchString = EditorGUILayout.TextField(_searchString);
            if (GUILayout.Button("Search"))
            {
                SearchTypes();
            }

            _selectedTypeIndex = EditorGUILayout.Popup("Select Type", _selectedTypeIndex, _filteredTypes.Select(t => t.Name).ToArray());
            if (_selectedType != _filteredTypes[_selectedTypeIndex])
            {
                _selectedType = _filteredTypes[_selectedTypeIndex];
                InitializeFields();
                
                // _fields = _selectedType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                // _dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_selectedType));
                //
                // _fieldCheckboxes = _fields.ToDictionary(f => f.Name, f => false);
            }

            EditorGUILayout.EndVertical();

            if (_selectedType == null)
            {
                EditorGUILayout.HelpBox("Please select a type", MessageType.Warning);
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
            
            if (_selectedSupportedType == SupportedType.SCRIPTABLE_OBJECT && GUILayout.Button("Export to ScriptableObjects"))
            {
                ExportToScriptableObjects();
            }

            if (GUILayout.Button("Import from CSV"))
            {
                ImportFromCSV();
            }

            EditorGUILayout.EndHorizontal();
            HandlePickSprite();
        }
        
        private void InitializeFields()
        {
            _fields = _selectedType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            _dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_selectedType));
            _uniqueIdentifiers = new List<string>();
            _fieldCheckboxes = _fields.ToDictionary(f => f.Name, f => false);
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal();

            if (_uniqueIdentifier)
            {
                GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(50));
            }

            foreach (var field in _fields)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(GetFieldWidth(field.FieldType)));
                GUILayout.Label(field.Name, EditorStyles.boldLabel);
                if (_fieldCheckboxes.ContainsKey(field.Name))
                {
                    _fieldCheckboxes[field.Name] = EditorGUILayout.Toggle(_fieldCheckboxes[field.Name], GUILayout.Width(15));
                }
                else
                {
                    _fieldCheckboxes[field.Name] = false;
                }
                EditorGUILayout.EndHorizontal();
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
                
                if (_uniqueIdentifier)
                {
                    if (!_customUniqueIdentifier)
                    {
                        GUILayout.Label(_uniqueIdentifiers[i], GUILayout.Width(50));
                    }
                    else
                    {
                        // user specific unique identifier as first column
                        //wEditorGUILayout.TextField((string)value, GUILayout.Width(GetFieldWidth(fieldType))
                        _uniqueIdentifiers[i] = EditorGUILayout.TextField(_uniqueIdentifiers[i], GUILayout.Width(50));
                    }
                        
                }
                
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

                if (_uniqueIdentifier)
                    _uniqueIdentifiers.RemoveAt(removeIndex);
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
            var newItem = Activator.CreateInstance(_selectedType);
            _dataList.Add(newItem);
            _uniqueIdentifiers.Add((_dataList.Count - 1).ToString());
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
        
        private void ExportToScriptableObjects()
        {
            if(_dataList.Count == 0 || _dataList == null)
            {
                Debug.LogWarning("Data list is empty");
                return;
            }
    
            if(_selectedSupportedType != SupportedType.SCRIPTABLE_OBJECT)
            {
                Debug.LogWarning("Selected type is not ScriptableObject");
                return;
            }
    
            string path = EditorUtility.OpenFolderPanel("Export ScriptableObjects", "", "");
    
            // Convert the absolute path to a relative project path
            path = path.Replace(Application.dataPath, "Assets");
    
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("Export path is empty");
                return;
            }

            for (int index = 0; index < _dataList.Count; index++)
            {
                var item = _dataList[index];
                var scriptableObject = ScriptableObject.CreateInstance(_selectedType);
        
                foreach (var field in _fields)
                {
                    var value = field.GetValue(item);
                    field.SetValue(scriptableObject, value);
                }

                string id = "";
                string valueNaming = "";
                string fileName = "";

                if (_uniqueIdentifier)
                {
                    // Unique identifier (first column)
                    id = _uniqueIdentifiers[index];
                }

                // Append selected fields to the name
                foreach (var field in _fields)
                {
                    if (_fieldCheckboxes[field.Name])
                    {
                        var value = field.GetValue(item);
                        valueNaming += $"_{value}";
                    }
                }
                
                if(!string.IsNullOrEmpty(_namePrefix))
                    fileName += $"{_namePrefix}_";
                
                if(!string.IsNullOrEmpty(id))
                    fileName += $"{id}_";
                
                if(!string.IsNullOrEmpty(_nameSuffix))
                    fileName += $"{_nameSuffix}_";
                
                if(!string.IsNullOrEmpty(valueNaming))
                    fileName += $"{valueNaming}";
                
                
                fileName = $"{fileName}.asset";
                fileName = fileName.Replace(" ", "_");

                string assetPath = Path.Combine(path, fileName);
        
                AssetDatabase.CreateAsset(scriptableObject, assetPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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

            var newDataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_selectedType));

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

                    object newItem;
                    
                    if(_selectedSupportedType == SupportedType.SCRIPTABLE_OBJECT)
                    {
                        newItem = CreateInstance(_selectedType);
                    }
                    else
                    {
                        newItem = Activator.CreateInstance(_selectedType);
                    }

                    // object newItem = Activator.CreateInstance(_selectedType);
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
                _uniqueIdentifiers.Add((_dataList.Count - 1).ToString());
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

        private void SearchTypes()
        {
            if (string.IsNullOrWhiteSpace(_searchString))
            {
                _filteredTypes = _selectedSupportedType == SupportedType.STRUCT ? new List<Type>(_structTypes) : new List<Type>(_scriptableObjectTypes);
            }
            else
            {
                _filteredTypes = (_selectedSupportedType == SupportedType.STRUCT ? _structTypes : _scriptableObjectTypes)
                    .Where(t => t.Name.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            _selectedTypeIndex = 0;
            if (_filteredTypes.Count > 0)
            {
                _selectedType = _filteredTypes[_selectedTypeIndex];
                _fields = _selectedType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                _dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_selectedType));
            }
            else
            {
                _selectedType = null;
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

        private List<Type> GetScriptableObjectTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(ScriptableObject)))
                .ToList();
        }
    }
}
