using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHierarchyInstantiator : MonoBehaviour
{
    public GameObject canvasPrefab; // Prefab for the canvas
    public Transform parentTransform; // Parent transform for the instantiated canvas hierarchy

    public static UIHierarchyInstantiator instance;
    private void Awake()
    {
        instance = this;
    }

    public string jsonFilePath;
    public Sprite spForBtns;
    public Sprite spForImgs;
    public Font fontText;
    
    // Method to instantiate the UI hierarchy from JSON
    public void InstantiateUIHierarchy()
    {
        
        if (string.IsNullOrEmpty(jsonFilePath))
        {
            Debug.LogError("JSON file path is null or empty.");
            return;
        }

        string jsonData = System.IO.File.ReadAllText(jsonFilePath);
        UIObject rootObject = JsonUtility.FromJson<UIObject>(jsonData);

        if (rootObject == null)
        {
            Debug.LogError("Failed to parse JSON data.");
            return;
        }

        // Instantiate the canvas
        GameObject canvasObject = Instantiate(canvasPrefab, parentTransform);
        RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();

        // Set canvas properties
        canvasObject.name = rootObject.name;
        canvasRectTransform.localPosition = new Vector3(rootObject.position.x, rootObject.position.y, rootObject.position.z);

        // Recursively instantiate UI objects
        InstantiateUIObjects(rootObject.components, canvasRectTransform);
    }

    // Recursive method to instantiate UI objects
    private void InstantiateUIObjects(List<UIObject> uiObjects, Transform parent)
    {
        if (uiObjects == null)
            return;

        foreach (UIObject uiObject in uiObjects)
        {
            // Create a new GameObject for the UI object
            GameObject newObject = new GameObject(uiObject.name);

            // Add RectTransform component to the new GameObject
            RectTransform rectTransform = newObject.AddComponent<RectTransform>();

            // Set position, rotation, scale, and other properties of the RectTransform
            rectTransform.localPosition = new Vector3(uiObject.position.x, uiObject.position.y, uiObject.position.z);
            rectTransform.localRotation = Quaternion.Euler(uiObject.rotation.x, uiObject.rotation.y, uiObject.rotation.z);
            rectTransform.localScale = new Vector3(uiObject.scale.x, uiObject.scale.y, uiObject.scale.z);
            // You can set rotation and scale similarly if needed

            switch (uiObject.type)
            {
                case "Button":
                    newObject.AddComponent<Image>();
                    newObject.GetComponent<Image>().sprite = spForBtns;
                    newObject.AddComponent<Button>();
                    newObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);
                    newObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500);
                    newObject.GetComponent<Image>().preserveAspect = true;
                    GameObject textForbtns = new GameObject();
                    textForbtns.AddComponent<Text>();
                    textForbtns.transform.SetParent(newObject.transform);
                    textForbtns.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);
                    textForbtns.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500);

                    textForbtns.GetComponent<Text>().font = fontText;
                    textForbtns.GetComponent<Text>().fontSize = 50;
                    textForbtns.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

                    newObject.GetComponent<Button>().targetGraphic = newObject.GetComponent<Image>();
                    newObject.GetComponentInChildren<Text>().text = uiObject.showMsg;
                    // You might want to set other properties of the Button component (e.g., onClick listeners)
                    break;
                case "Text":
                    Text textComponent = newObject.AddComponent<Text>();
                    newObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500); 
                    newObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500);
                    // Set text properties (e.g., font, font size, color)
                    // Example:
                    textComponent.font = fontText;
                    textComponent.fontSize = 60;

                    textComponent.text = uiObject.showMsg;
                    newObject.AddComponent<Outline>();
                    break;
                case "Image":
                    newObject.AddComponent<Image>();
                    newObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);
                    newObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500);
                    newObject.GetComponent<Image>().sprite = spForImgs;
                    newObject.GetComponent<Image>().preserveAspect = true;
                    // Set image properties (e.g., sprite, color)
                    break;
                    // Add cases for other types of UI components you support
            }


            // Handle nested UI objects recursively
            InstantiateUIObjects(uiObject.components, newObject.transform);

            // Set the parent of the new GameObject to the specified parent transform
            newObject.transform.SetParent(parent, false); // Set 'false' to preserve world position and rotation
        }
    }

}
