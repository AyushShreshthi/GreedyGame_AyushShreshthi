using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


[Serializable]
public class UIObject
{
    public string name;
    public string type;
    public string showMsg;
    public TransformData position;
    public TransformData rotation;
    public TransformData scale;
    public ColorData color;
    public List<UIObject> components;


    public string SerializeToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // Static method to deserialize JSON string to UIObject
    public static UIObject DeserializeFromJson(string json)
    {
        return JsonUtility.FromJson<UIObject>(json);
    }


}

[Serializable]
public class TransformData
{
    public float x, y, z, w; // Note: 'w' is used for quaternion rotation
}

[Serializable]
public class ColorData
{
    public float r, g, b, a;
}


public class UIEditorWindow : EditorWindow
{
    private UIObject rootUIObject;
    public string jsonFilePath = "";

    private string newName = "New UI Object";
    private string newType = "Button"; // Example default type
    private string newText = "Text"; // Example default type
    private TransformData newPosition = new TransformData { x = 0, y = 0, z = 0 };
    private TransformData newScale = new TransformData { x = 1, y = 1, z = 1 };
    private ColorData newColor = new ColorData { r = 255, g = 255, b = 255, a = 255 };


    [MenuItem("Window/UI Editor")]
    public static void ShowWindow()
    {
        GetWindow<UIEditorWindow>("UI Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("UI Object Editor", EditorStyles.boldLabel);

        GUILayout.Label("Create New UI Object", EditorStyles.boldLabel);

        newName = EditorGUILayout.TextField("Name", newName);
        newType = EditorGUILayout.TextField("Type", newType);
        newText = EditorGUILayout.TextField("Text", newText);
        // Fields for position
        newPosition.x = EditorGUILayout.FloatField("Position X", newPosition.x);
        newPosition.y = EditorGUILayout.FloatField("Position Y", newPosition.y);
        newPosition.z = EditorGUILayout.FloatField("Position Z", newPosition.z);

        // Fields for color
        newColor.r = EditorGUILayout.Slider("Color R", newColor.r, 0, 255);
        newColor.g = EditorGUILayout.Slider("Color G", newColor.g, 0, 255);
        newColor.b = EditorGUILayout.Slider("Color B", newColor.b, 0, 255);
        newColor.a = EditorGUILayout.Slider("Alpha", newColor.a, 0, 1);

        // Fields for scale
        newScale.x = EditorGUILayout.FloatField("Scale X", newScale.x);
        newScale.y = EditorGUILayout.FloatField("Scale Y", newScale.y);
        newScale.z = EditorGUILayout.FloatField("Scale Z", newScale.z);

        if (GUILayout.Button("Add New UI Object"))
        {
            AddNewUIObject();
        }


        if (GUILayout.Button("Load JSON"))
        {
            LoadJson();
        }

        if (rootUIObject != null)
        {
            // Display and edit the UI Object properties here
            rootUIObject.name = EditorGUILayout.TextField("Root Object Name", rootUIObject.name);

            if (GUILayout.Button("Save JSON"))
            {
                SaveJson();
            }
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load UI JSON", "", "json");
        if (path.Length != 0)
        {
            jsonFilePath = path;
            string dataAsJson = File.ReadAllText(path);
            UIHierarchyInstantiator.instance.jsonFilePath = path;
            rootUIObject = JsonUtility.FromJson<UIObject>(dataAsJson);
        }
    }

    private void SaveJson()
    {
        if (!string.IsNullOrEmpty(jsonFilePath))
        {
            string dataAsJson = JsonUtility.ToJson(rootUIObject, true);
            File.WriteAllText(jsonFilePath, dataAsJson);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("No JSON file loaded or specified for saving.");
        }
    }

    private void AddNewUIObject()
    {
        if (rootUIObject == null)
        {
            rootUIObject = new UIObject(); // Initialize if null
        }

        if (rootUIObject.components == null)
        {
            rootUIObject.components = new List<UIObject>();
        }

        UIObject newObject = new UIObject
        {
            name = newName,
            type = newType,
            position = newPosition,
            color = newColor,
            scale=newScale,
            showMsg=newText
            // Set other properties as needed
        };

        rootUIObject.components.Add(newObject);
    }
}


