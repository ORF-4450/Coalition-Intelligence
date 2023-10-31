using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class GeneralVariableModifier<T> : MonoBehaviour where T : GeneralVariable
{
    [SerializeField] private TextMeshProUGUI nameVisualizer;

    protected abstract T Variable { get; set; }

    private void Start()
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
        Instantiate(Variable);
        //gameObjectGrid.AddGridObject(Variable.gameObject);
    }
    protected void DeleteVariable()
    {
        //gameObjectGrid.RemoveGridObject(Variable.gameObject);
        Destroy(gameObject);
    }
}
