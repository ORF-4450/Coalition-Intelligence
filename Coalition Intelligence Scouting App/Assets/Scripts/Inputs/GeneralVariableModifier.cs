using TMPro;
using UnityEngine;

public abstract class GeneralVariableModifier<T> : MonoBehaviour where T : GeneralVariable
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private TextMeshProUGUI nameVisualizer;

    protected T _Variable;
    protected abstract T Variable { get; set; }

    private void OnEnable()
    {
        SetValuesInMenu();
    }

    protected void SetValuesInMenu()
    {
        if (Variable == null) SetToDefaultValues();
        else SetToVariableValues();
    }
    protected virtual void SetToVariableValues()
    {
        nameVisualizer.text = Variable.infoName;
    }
    protected virtual void SetToDefaultValues()
    {
        nameVisualizer.text = "";
    }
    protected virtual void SaveVariable()
    {
        Variable.infoName = nameVisualizer.text;
    }

    protected void CreateVariable()
    {
        Instantiate(prefab);
    }
    protected void DeleteVariable()
    {
        Destroy(gameObject);
    }
}
