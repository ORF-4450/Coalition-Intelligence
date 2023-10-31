using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownVariableModifier : GeneralVariableModifier<DropdownVariable>
{
    protected override DropdownVariable Variable { get => Variable; set => Variable = value; }
    // Dropdown Options need a scrolling field, infinite downward.
    // If you child the objects under the scroll bar, might as well use that
    protected Transform visualizedDropdownOptionsParent;
    protected GameObject visualizedDropdownOptionPrefab;
    protected static List<DropdownOptionVisualizer> dropdownOptionVisualizers = new();
    protected Vector2 dropdownOptionInitialPosition;

    private void Start()
    {
        // Calculate where dropdownOptionInitialPosition is
        // x => identical as visualizedDropdownOptionsParent.position.x
        // y => visualizedDropdownOptionsParent w/ offset (like half of prefab.localScale.y)
        // Maybe below works? Depends on how it's made in Unity
        dropdownOptionInitialPosition =
            new Vector2(visualizedDropdownOptionsParent.position.x,
            visualizedDropdownOptionsParent.position.y
                - visualizedDropdownOptionPrefab.transform.localScale.y / 2);
    }

    protected override void SetToVariableValues()
    {
        base.SetToVariableValues();
        if (dropdownOptionVisualizers.Count > Variable.dropdown.options.Count)
            DestroyVisualizedDropdownOptions(Variable.dropdown.options.Count - 1,
                                             dropdownOptionVisualizers.Count - 1);
        for (int i = 0; i < dropdownOptionVisualizers.Count; i++)
            dropdownOptionVisualizers[i].textVisualizer.text = Variable.dropdown.options[i].text;
    }
    protected override void SetToDefaultValues()
    {
        base.SetToDefaultValues();
        for (int i = 0; i < dropdownOptionVisualizers.Count; i++)
            dropdownOptionVisualizers[i].textVisualizer.text = "";
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
        DropdownOptionVisualizer visualizedDropdownOptionScript = 
            visualizedDropdownOption.GetComponent<DropdownOptionVisualizer>();

        dropdownOptionVisualizers.Add(visualizedDropdownOptionScript);

        visualizedDropdownOptionScript.textVisualizer.text = info;
        visualizedDropdownOption.transform.position = 
            new Vector2(dropdownOptionInitialPosition.x,
                        dropdownOptionInitialPosition.y - 
                        (dropdownOptionVisualizers.Count - 1) * 
                        visualizedDropdownOptionPrefab.transform.localScale.y);
    }

    protected void DestroyVisualizedDropdownOptions()
    {
        DestroyVisualizedDropdownOptions(0, visualizedDropdownOptionsParent.childCount - 1);
    }
    protected void DestroyVisualizedDropdownOptions(int start, int end)
    {
        dropdownOptionVisualizers.Clear();
        for (int i = end; i > start; i--)
            Destroy(visualizedDropdownOptionsParent.GetChild(i));
    }

    public static void Move(DropdownOptionVisualizer dropdownOptionVisualizer, int newIndex)
    {
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
        public TMP_InputField indexWriter;
        public bool selected;
        public TMP_InputField textVisualizer;

        public DropdownOptionVisualizer()
        {
            selected = false;
            textVisualizer.text = "";
            indexWriter.onValueChanged.AddListener(e => Move(this, int.Parse(indexWriter.text)));
        }
    }
}
