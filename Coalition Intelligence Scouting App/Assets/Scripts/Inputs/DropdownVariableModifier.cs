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


    protected override void OnEnable()
    {
        destroy.interactable = !(Variable == null);

        base.OnEnable();
        addOptionButton.onClick.AddListener(() => AddVisualizedDropdownOption(addOptionInputField.text));
        // addOptionInputField.onEndEdit.AddListener(() => AddVisualizedDropdownOption());
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        addOptionButton.onClick.RemoveAllListeners();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        addOptionButton.onClick.RemoveAllListeners();
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
        {
            Variable.dropdown.options.Add(new TMP_Dropdown.OptionData(dropdownOptionVisualizers[i].textVisualizer.text));
        }
    }

    protected void AddVisualizedDropdownOption()
    {
        AddVisualizedDropdownOption("");
    }

    public void AddVisualizedDropdownOptionFromInput()
    {
        if (addOptionInputField.text.EndsWith("\n"))
        {
            addOptionInputField.text = addOptionInputField.text.Replace("\n", "");
            AddVisualizedDropdownOption(addOptionInputField.text);
        }
    }

    protected void AddVisualizedDropdownOption(string info)
    {
        DropdownOptionVisualizer visualizedDropdownOption = Instantiate(visualizedDropdownOptionPrefab, transform).GetComponent<DropdownOptionVisualizer>();
        visualizedDropdownOption.textVisualizer.text = info;
        dropdownOptionVisualizers.Add(visualizedDropdownOption);

        addOptionInputField.text = "";
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

    public void Move(DropdownOptionVisualizer dropdownOptionVisualizer, int newIndex)
    {
        newIndex = Mathf.Clamp(newIndex, 0, dropdownOptionVisualizers.Count - 1);
        dropdownOptionVisualizer.transform.SetSiblingIndex(newIndex);
        dropdownOptionVisualizers.Remove(dropdownOptionVisualizer);
        dropdownOptionVisualizers.Insert(newIndex, dropdownOptionVisualizer);
    }
}
