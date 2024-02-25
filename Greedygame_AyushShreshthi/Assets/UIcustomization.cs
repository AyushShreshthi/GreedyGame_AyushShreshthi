using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class UIcustomization : EditorWindow
{
    private List<GameObject> selectedUIObjects = new List<GameObject>();

    private string newName = "New UI Object";
    private string newType = "Button"; // Example default type
    private string newText = "Text"; // Example default type
    private TransformData newPosition = new TransformData { x = 0, y = 0, z = 0 };
    private TransformData newScale = new TransformData { x = 1, y = 1, z = 1 };
    private ColorData newColor = new ColorData { r = 255, g = 255, b = 255, a = 255 };


    [MenuItem("Window/UIcustomization")]
    public static void ShowWindow()
    {
        GetWindow<UIcustomization>("UIcustomization");
    }

    private void OnGUI()
    {
        GUILayout.Label("Selected UI Objects:", EditorStyles.boldLabel);


        // Display UI objects selected by the user
        foreach (GameObject uiObject in selectedUIObjects)
        {
            EditorGUILayout.ObjectField(uiObject, typeof(GameObject), true);
        }

        GUILayout.Space(20);

        // Button to add selected UI objects
        if (GUILayout.Button("Add Selected UI Objects"))
        {
            GameObject[] selection = Selection.gameObjects;
            foreach (GameObject obj in selection)
            {
                // Check if the selected object has a UI component
                if (obj.GetComponent<RectTransform>() != null)
                {
                    selectedUIObjects.Add(obj);
                }
            }
        }

        GUILayout.Space(20);

        // Display controls to modify properties of selected UI objects
        GUILayout.Label("Modify Properties:", EditorStyles.boldLabel);

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

        GUILayout.Space(20);

        // Button to save changes to JSON file
        if (GUILayout.Button("Save Changes to JSON"))
        {
            SaveChangesToJson();
        }
    }

    private void SaveChangesToJson()
    {
        // Serialize modified UI objects to JSON format
        List<UIObject> uiObjectsData = new List<UIObject>();
        foreach (GameObject uiObject in selectedUIObjects)
        {
            UIObject dataToUpdate = uiObjectsData.Find(objData => objData.name == uiObject.name);

            if (dataToUpdate != null)
            {
                // Update properties of the UIObjectData instance with the modified properties of the UI object
                dataToUpdate.position = newPosition;
                dataToUpdate.color = newColor;
                dataToUpdate.scale = newScale;
            }
            
        }

        // Serialize uiObjectsData list to JSON string
        string json = JsonUtility.ToJson(uiObjectsData);

        // Write JSON string to the original JSON file
        string filePath = "Assets/ui_templates.json";
        File.WriteAllText(filePath, json);

        Debug.Log("Changes saved to JSON file.");
    }
}
