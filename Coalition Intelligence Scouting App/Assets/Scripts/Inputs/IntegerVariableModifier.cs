using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntegerVariableModifier : GeneralVariableModifier<IntegerVariable>
{
    protected override IntegerVariable Variable { get => _Variable; set => _Variable = value; }

    [SerializeField] protected Toggle minimumValueExistsVisualizer;
    [SerializeField] protected TMP_InputField minimumValueVisualizer;
    [SerializeField] protected Toggle maximumValueExistsVisualizer;
    [SerializeField] protected TMP_InputField maximumValueVisualizer;
    // [SerializeField] protected TMP_InputField valueVisualizer;

    protected override void Awake()
    {
        base.Awake();
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
        // valueVisualizer.text = Variable.value.ToString();
    }
    protected override void SetToDefaultValues()
    {
        base.SetToDefaultValues();
        minimumValueVisualizer.interactable = minimumValueExistsVisualizer.isOn = true;
        minimumValueVisualizer.text = "0";
        maximumValueVisualizer.interactable = maximumValueExistsVisualizer.isOn = false;
        maximumValueVisualizer.text = "0";
        // valueVisualizer.text = "0";
    }
    protected override void SaveVariable()
    {
        base.SaveVariable();
        Variable.minimumValueExists = minimumValueExistsVisualizer.isOn;
        Variable.minimumValue = int.Parse(minimumValueVisualizer.text);
        Variable.maximumValueExists = maximumValueExistsVisualizer.isOn;
        Variable.maximumValue = int.Parse(maximumValueVisualizer.text);
        // Variable.value = int.Parse(valueVisualizer.text);
        // Variable.valueVisualizer.text = Variable.value.ToString();
    }
}
