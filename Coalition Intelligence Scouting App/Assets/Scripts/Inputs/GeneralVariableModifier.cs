using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class GeneralVariableModifier<T> : MonoBehaviour where T : GeneralVariable
{
    [SerializeField] private GameObject prefab;
    protected T defaultVariable;
    [SerializeField] private TMP_InputField nameVisualizer;
    
    [SerializeField] private GameObject menuLocation;

    [SerializeField] public Button saveChanges;
    [SerializeField] public Button cancelChanges;
    [SerializeField] public Button destroy;
    [SerializeField] public MenuSwapper typeMenus;

    protected virtual void Awake()
    {
        defaultVariable = prefab.GetComponent<T>();
    }

    protected virtual void OnDestroy()
    {
        saveChanges.onClick.RemoveAllListeners();
        cancelChanges.onClick.RemoveAllListeners();
        destroy.onClick.RemoveAllListeners();
    }

    protected T _Variable;
    protected abstract T Variable { get; set; }

    protected virtual void OnEnable()
    {
        SetValuesInMenu();

        saveChanges.onClick.AddListener(() => CreateVariable());
        cancelChanges.onClick.AddListener(() => CancelChanges());
        destroy.onClick.AddListener(() => Destroy());
    }

    protected virtual void OnDisable()
    {
        saveChanges.onClick.RemoveAllListeners();
        cancelChanges.onClick.RemoveAllListeners();
        destroy.onClick.RemoveAllListeners();   
    }

    protected void SetValuesInMenu()
    {
        if (Variable == null) SetToDefaultValues();
        else SetToVariableValues();
    }
    protected virtual void SetToVariableValues()
    {
        Debug.Log("Var");
        nameVisualizer.text = Variable.infoName;
    }
    protected virtual void SetToDefaultValues()
    {
        Debug.Log("Def");
        nameVisualizer.text = "";
    }
    protected virtual void SaveVariable()
    {
        if (Variable == null) Variable = defaultVariable;
        Variable.infoName = nameVisualizer.text;
    }
    protected void CreateVariable()
    {
        // if gameObject.isVisible;
        SaveVariable();
        GameObject instantiatedObject = Instantiate(prefab, menuLocation.transform);
        CopyComponent<T>(Variable, instantiatedObject);
        ExitMenu();
    }
    protected void DeleteVariable()
    {
        Destroy(Variable.gameObject);
        ExitMenu();
    }

    protected virtual void ExitMenu()
    {
        Variable = null;
        SetToDefaultValues();
        typeMenus.ChangeMenu(typeMenus.defaultMenu);
    }
    protected virtual void CancelChanges()
    {
        ExitMenu();
    }
    protected void Destroy()
    {

    }

    Two CopyComponent<Two>(Two original, GameObject destination) where Two : T
    {
        System.Type type = typeof(Two);
        Two copy = destination.AddComponent<Two>();
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
        return copy as Two;
    }
}
