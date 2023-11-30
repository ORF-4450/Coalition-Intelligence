using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ColorHolder : MonoBehaviour
{
    [SerializeField] public List<ColorOption> ColorOptions;

    [SerializeField] int currentColorIndex = 1;
    [SerializeField] public GameObject canvas;
    [SerializeField] public GameObject cameraObject;
    [SerializeField] public TMP_Dropdown colorSelector;

    void Awake()
    {
        SetColor();
    }
    public void SetColor()
    {
        currentColorIndex = colorSelector.value;
        canvas.BroadcastMessage("ApplyColors", ColorOptions[currentColorIndex], SendMessageOptions.DontRequireReceiver);
        cameraObject.BroadcastMessage("ApplyColors", ColorOptions[currentColorIndex], SendMessageOptions.DontRequireReceiver);
    }

    public void SetColor(GameObject parentObject)
    {
        parentObject.BroadcastMessage("ApplyColors", ColorOptions[currentColorIndex], SendMessageOptions.DontRequireReceiver);
    }
}

[System.Serializable]
public class ColorOption
{
    [SerializeField] public string name;
    [SerializeField] public Color primaryColor;
    [SerializeField] public Color secondaryColor;
    [SerializeField] public Color backgroundColor;
}