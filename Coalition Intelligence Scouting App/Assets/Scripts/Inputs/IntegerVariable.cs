using TMPro;
using System;

public class IntegerVariable : GeneralVariable
{
    public TextMeshProUGUI valueVisualizer;
    public bool minimumValueExists;
    public int minimumValue;
    public bool maximumValueExists;
    public int maximumValue;
    public int value { get => value; set => Math.Clamp(value, minimumValue, maximumValue); }
    public void ChangeValue(int change) { value = Math.Clamp(value + change, 0, maximumValue); }
}
