using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    [SerializeField] public int currentColorIndex;
    [SerializeField] public List<ColorOption> ColorOptions;
    public ColorOption currentColor;
    void SetColor(int colorIndex)
    {
        currentColor = ColorOptions[colorIndex - 1];
    }
    [EditorCools.Button]
    private void changeColor()
    {
        SetColor(currentColorIndex);
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