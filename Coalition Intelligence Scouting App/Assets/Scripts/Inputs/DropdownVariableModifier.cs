using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownVariableModifier : GeneralVariableModifier<DropdownVariable>
{
    protected override DropdownVariable Variable { get => _Variable; set => _Variable = value; }
    protected Transform visualizedDropdownOptionsParent;
    protected GameObject visualizedDropdownOptionPrefab;
    protected static List<DropdownOptionVisualizer> dropdownOptionVisualizers = new();

    protected override void SetToVariableValues()
    {
        base.SetToVariableValues();
        for (int i = 0; i < dropdownOptionVisualizers.Count; i++)
            dropdownOptionVisualizers[i].textVisualizer.text = Variable.dropdown.options[i].text;
    }
    protected override void SetToDefaultValues()
    {
        base.SetToDefaultValues();
        ClearDropdownOptions();
    }
    protected override void SaveVariable()
    {
        base.SaveVariable();
        Variable.dropdown.ClearOptions();
        for (int i = 0; i < dropdownOptionVisualizers.Count; i++)
            Variable.dropdown.options.Add(new TMP_Dropdown.OptionData(dropdownOptionVisualizers[i].textVisualizer.text));
    }

    protected void AddVisualizedDropdownOption()
    {
        AddVisualizedDropdownOption("");
    }
    protected void AddVisualizedDropdownOption(string info)
    {
        // Think this would work but dunno. Would like to make better
        GameObject visualizedDropdownOption = Instantiate(visualizedDropdownOptionPrefab);
        DropdownOptionVisualizer visualizedDropdownOptionScript = new DropdownOptionVisualizer();

        visualizedDropdownOptionScript.gameObject = visualizedDropdownOption;
        visualizedDropdownOptionScript.textVisualizer.text = info;

        dropdownOptionVisualizers.Add(visualizedDropdownOptionScript);
    }

    protected void ClearDropdownOptions()
    {
        for (int i = dropdownOptionVisualizers.Count; i >= 0; i--)
            DestroyDropdownOption(i);
    }
    protected void DestroyDropdownOption(int index)
    {
        Destroy(dropdownOptionVisualizers[index].gameObject);
        dropdownOptionVisualizers.RemoveAt(index);
    }

    public static void Move(DropdownOptionVisualizer dropdownOptionVisualizer, int newIndex)
    {
        if (newIndex < 0) newIndex = 0;
        if (newIndex >= dropdownOptionVisualizers.Count) newIndex = dropdownOptionVisualizers.Count - 1;
        int index = dropdownOptionVisualizers.IndexOf(dropdownOptionVisualizer);

        dropdownOptionVisualizers.RemoveAt(index);

        if (newIndex > index) newIndex--;
        // the actual index could have shifted due to the removal

        dropdownOptionVisualizers.Insert(newIndex, dropdownOptionVisualizer);
    }

    public class DropdownOptionVisualizer
    {
        // Lock this to only allow whole number inputs so maybe use TextValidator
        // and remove everything except 0123456789
        public GameObject gameObject;
        public int index { get => dropdownOptionVisualizers.IndexOf(this); }
        public TMP_InputField textVisualizer;
        private Button decreaseIndex;
        private Button increaseIndex;

        public DropdownOptionVisualizer()
        {
            textVisualizer.text = "";
            decreaseIndex.onClick.AddListener(() => Move(this, index - 1));
            increaseIndex.onClick.AddListener(() => Move(this, index + 1));
        }
    }
}
