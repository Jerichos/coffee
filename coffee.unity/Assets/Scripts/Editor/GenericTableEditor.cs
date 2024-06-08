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
public abstract class GenericTableEditorWindow : EditorWindow
{
    protected Vector2 scrollPosition;
    protected FieldInfo[] fields;
    protected abstract Type StructType { get; }
    protected abstract IList DataList { get; }

    private bool _showSpriteInField = false;
    
    private int pickerControlID = -1; // Control ID for the object picker
    private object pickerItem; // Item currently being edited
    private FieldInfo pickerField; // Field currently being edited

    protected void OnEnable()
    {
        fields = StructType.GetFields(BindingFlags.Public | BindingFlags.Instance);
    }

    protected void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        _showSpriteInField = EditorGUILayout.Toggle("Show Sprites in Fields", _showSpriteInField);
        EditorGUILayout.EndHorizontal();
        
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
        foreach (var field in fields)
        {
            GUILayout.Label(field.Name, EditorStyles.boldLabel, GUILayout.Width(GetFieldWidth(field.FieldType)));
        }
        GUILayout.Label("Actions", EditorStyles.boldLabel, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
    }

    private void DrawRows()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        int removeIndex = -1;

        for (int i = 0; i < DataList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            var item = DataList[i];
            foreach (var field in fields)
            {
                DrawField(ref item, field);
            }
            if (GUILayout.Button("Remove", GUILayout.Width(100)))
            {
                removeIndex = i;
            }
            DataList[i] = item;
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (removeIndex >= 0)
        {
            DataList.RemoveAt(removeIndex);
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

                    if (GUILayout.Button("",GUILayout.Width(GetFieldWidth(fieldType)), GUILayout.Height(GetFieldWidth(fieldType))))
                    {
                        EditorGUIUtility.ShowObjectPicker<Sprite>(sprite, false, null, 0);
                        // pickerControlID = GUIUtility.GetControlID(FocusType.Keyboard); // Unique ID for object picker
                        pickerItem = item;
                        pickerField = field;
                        // EditorGUIUtility.ShowObjectPicker<Sprite>(sprite, false, "", pickerControlID);
                        // Debug.Log("Sprite button clicked: " + sprite.name + " pickerControlID: " + pickerControlID + " pickerItem: " + pickerItem + " pickerField: " + pickerField);
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
        // Add more field types as needed
        else
        {
            GUILayout.Label("Unsupported Type", GUILayout.Width(GetFieldWidth(fieldType)));
        }
    }

    private void AddNewRow()
    {
        var newItem = Activator.CreateInstance(StructType);
        DataList.Add(newItem);
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
            writer.WriteLine(string.Join(",", fields.Select(f => f.Name)));

            // Write data
            foreach (var item in DataList)
            {
                var values = fields.Select(f =>
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

        var newDataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(StructType));
    
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

                var newItem = Activator.CreateInstance(StructType);
                for (int i = 0; i < fieldNames.Length; i++)
                {
                    var field = fields.FirstOrDefault(f => f.Name == fieldNames[i]);
                    if (field == null) continue;

                    object value = ConvertValue(field.FieldType, values[i].Replace(";", ","));
                    field.SetValue(newItem, value);
                }

                newDataList.Add(newItem);
            }
        }

        // foreach (var item in newDataList)
        // {
        //     DataList.Add(item);
        // }
        DataList.Clear();
        foreach (var item in newDataList)
        {
            DataList.Add(item);
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
            Debug.Log("Event.current.commandName: " + e.commandName + ", pickerControlID: " + pickerControlID);
            Sprite selectedSprite = (Sprite)EditorGUIUtility.GetObjectPickerObject();
            if (selectedSprite != null)
            {
                Debug.Log("Selected sprite: " + selectedSprite.name);
            }
            else
            {
                Debug.Log("No sprite selected");
            }
            
            if (pickerItem != null && pickerField != null)
            {
                int index = DataList.IndexOf(pickerItem);
                Debug.Log("index: " + index);
                pickerField.SetValue(pickerItem, selectedSprite);
                DataList[index] = pickerItem; // Update the item in the list
                Debug.Log("Sprite field updated ! " + pickerItem + " " + pickerField + " " + selectedSprite);
                Repaint(); // Refresh the editor window
            }
            else
            {
                Debug.LogWarning("Picker item or field is null");
            }
        }
    }

}
}
