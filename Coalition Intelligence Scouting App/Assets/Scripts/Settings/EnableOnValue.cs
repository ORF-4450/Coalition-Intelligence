using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnableOnValue : MonoBehaviour
{

    public List<ObjectElement> objects;
    TMP_Dropdown dropdown;

    public void OnEnable()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        Check();
    }
    
    public void Check()
    {
        Trigger(dropdown.options[dropdown.value].text);
    }

    public void Trigger(string input)
    {
        foreach (ObjectElement element in objects)
        {
            bool enable = (input == element.enableOnValue);
            if (enable)
            {
                element.objectToChange.SetActive(element.enable);
            } else {
                element.objectToChange.SetActive(!element.enable);
            }
        }
    }
}

[System.Serializable]
public class ObjectElement
{
    public GameObject objectToChange;
    public bool enable;
    public string enableOnValue;
}