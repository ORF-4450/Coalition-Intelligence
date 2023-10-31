using TMPro;
using UnityEngine.UI;

public class IntegerVariableModifier : GeneralVariableModifier<IntegerVariable>
{
    protected override IntegerVariable Variable { get => Variable; set => Variable = value; }

    protected Toggle minimumValueExistsVisualizer;
    protected TMP_InputField minimumValueVisualizer;
    protected Toggle maximumValueExistsVisualizer;
    protected TMP_InputField maximumValueVisualizer;
    protected TMP_InputField valueVisualizer;

    private void Awake()
    {
        minimumValueExistsVisualizer.onValueChanged.AddListener
            (e => minimumValueVisualizer.interactable = minimumValueExistsVisualizer.isOn);
        maximumValueExistsVisualizer.onValueChanged.AddListener
            (e => maximumValueVisualizer.interactable = maximumValueExistsVisualizer.isOn);
    }

    protected override void SetToVariableValues()
    {
        base.SetToVariableValues();
        minimumValueExistsVisualizer.isOn = Variable.minimumValueExists;
        minimumValueVisualizer.text = Variable.minimumValue.ToString();
        maximumValueExistsVisualizer.isOn = Variable.maximumValueExists;
        maximumValueVisualizer.text = Variable.maximumValue.ToString();
        valueVisualizer.text = Variable.value.ToString();
    }
    protected override void SetToDefaultValues()
    {
        base.SetToDefaultValues();
        minimumValueExistsVisualizer.isOn = true;
        minimumValueVisualizer.text = "0";
        maximumValueExistsVisualizer.isOn = true;
        maximumValueVisualizer.text = int.MaxValue.ToString();
        valueVisualizer.text = "0";
    }
    protected override void SaveVariable()
    {
        base.SaveVariable();
        Variable.minimumValueExists = minimumValueExistsVisualizer.isOn;
        Variable.minimumValue = int.Parse(minimumValueVisualizer.text);
        Variable.maximumValueExists = maximumValueExistsVisualizer.isOn;
        Variable.maximumValue = int.Parse(maximumValueVisualizer.text);
        Variable.value = int.Parse(valueVisualizer.text);
        Variable.valueVisualizer.text = Variable.value.ToString();
    }
}
