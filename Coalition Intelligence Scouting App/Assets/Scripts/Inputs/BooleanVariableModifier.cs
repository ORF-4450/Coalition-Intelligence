using UnityEngine;
using UnityEngine.UI;

public class BooleanVariableModifier : GeneralVariableModifier<BooleanVariable>
{
    protected override BooleanVariable Variable { get => _Variable; set => _Variable = value; }

    [SerializeField] Toggle booleanVisualizer;

    protected override void SetToVariableValues()
    {
        base.SetToVariableValues();
        booleanVisualizer.isOn = Variable.value.isOn;
    }
    protected override void SetToDefaultValues()
    {
        base.SetToDefaultValues();
        booleanVisualizer.isOn = false;
    }
    protected override void SaveVariable()
    {
        base.SaveVariable();
        Variable.value.isOn = booleanVisualizer.isOn;
    }
}
