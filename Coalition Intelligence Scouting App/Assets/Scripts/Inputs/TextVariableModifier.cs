using TMPro;

public class TextVariableModifier : GeneralVariableModifier<TextVariable>
{
    protected override TextVariable Variable { get => Variable; set => Variable = value; }
    protected TMP_InputField textVisualizer;

    protected override void SetToVariableValues()
    {
        base.SetToVariableValues();
        textVisualizer.text = Variable.inputField.text;
    }
    protected override void SetToDefaultValues()
    {
        base.SetToDefaultValues();
        textVisualizer.text = "";
    }
    protected override void SaveVariable()
    {
        base.SaveVariable();
        Variable.inputField.text = textVisualizer.text;
    }
}
