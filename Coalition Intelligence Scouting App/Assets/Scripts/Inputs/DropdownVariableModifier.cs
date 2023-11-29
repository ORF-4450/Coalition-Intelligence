using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownVariableModifier : GeneralVariableModifier<DropdownVariable>
{
    protected override DropdownVariable Variable { get => _Variable; set => _Variable = value; }
    protected Transform visualizedDropdownOptionsParent;
    [SerializeField] protected GameObject visualizedDropdownOptionPrefab;
    public List<DropdownOptionVisualizer> dropdownOptionVisualizers = new();

    [SerializeField] public Button addOptionButton;

    [SerializeField] TMP_InputField addOptionInputField;

    void Awake()
    {
        addOptionButton.onClick.AddListener(() => AddVisualizedDropdownOption(addOptionInputField.text));
    }

    void OnDestroy()
    {
        addOptionButton.onClick.RemoveListener(() => AddVisualizedDropdownOption(addOptionInputField.text));
    }

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
        DropdownOptionVisualizer visualizedDropdownOption = Instantiate(visualizedDropdownOptionPrefab, transform).GetComponent<DropdownOptionVisualizer>();

        visualizedDropdownOption.textVisualizer.text = info;

        dropdownOptionVisualizers.Add(visualizedDropdownOption);
    }

    protected void ClearDropdownOptions()
    {
        for (int i = dropdownOptionVisualizers.Count; i > 0; i--)
        {
            DestroyDropdownOption(i);
        }
    }
    public void DestroyDropdownOption(int index)
    {
        Destroy(dropdownOptionVisualizers[index].gameObject);
        dropdownOptionVisualizers.RemoveAt(index);
    }

    public void Move(DropdownOptionVisualizer dropdownOptionVisualizer, int newIndex) =>
        dropdownOptionVisualizer.transform.SetSiblingIndex(newIndex);
}
