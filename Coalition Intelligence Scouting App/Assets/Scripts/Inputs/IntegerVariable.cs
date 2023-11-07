using System;
using TMPro;

public class IntegerVariable : GeneralVariable
{
    public TextMeshProUGUI valueVisualizer;
    public bool minimumValueExists;
    public int minimumValue;
    public bool maximumValueExists;
    public int maximumValue;
    public int value;
    public void ChangeValue(int change)
    {
        value = Math.Clamp(value + change,
                           minimumValueExists ? minimumValue : int.MinValue,
                           maximumValueExists ? maximumValue : int.MaxValue);
        valueVisualizer.text = value.ToString();
    }
}
