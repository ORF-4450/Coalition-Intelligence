using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownOptionVisualizer : MonoBehaviour
{
    public int index { get => dropdownVariableModifier.dropdownOptionVisualizers.IndexOf(this); }
    [HideInInspector] public DropdownVariableModifier dropdownVariableModifier;
    [SerializeField] public TMP_InputField textVisualizer;
    [SerializeField] private Button increaseIndex;
    [SerializeField] private Button decreaseIndex;
    [SerializeField] private Button delete;

    private void Awake()
    {
        dropdownVariableModifier = transform.parent.GetComponent<DropdownVariableModifier>();
        textVisualizer.text = "";
        decreaseIndex.onClick.AddListener(() => dropdownVariableModifier.Move(this, index - 1));
        increaseIndex.onClick.AddListener(() => dropdownVariableModifier.Move(this, index + 1));
        delete.onClick.AddListener(() => dropdownVariableModifier.DestroyDropdownOption(index));
    }

    private void OnDestroy()
    {
        textVisualizer.text = "";
        decreaseIndex.onClick.RemoveListener(() => dropdownVariableModifier.Move(this, index - 1));
        increaseIndex.onClick.RemoveListener(() => dropdownVariableModifier.Move(this, index + 1));
        delete.onClick.RemoveListener(() => dropdownVariableModifier.DestroyDropdownOption(index));
    }
}
