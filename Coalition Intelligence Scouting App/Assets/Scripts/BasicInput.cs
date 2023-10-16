using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInput : MonoBehaviour
{
    string key = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class DropdownInput : BasicInput
{
    List<DropdownComponent> dropdownComponents = new();
}

[System.Serializable]
public class DropdownComponent
{
    string name = null;
}

[System.Serializable]
public class BooleanInput : BasicInput
{
    bool clicked = false;
}

[System.Serializable]
public class IntegerInput : BasicInput
{
    int number;
    int defaultNumber = 0;

    void Start()
    {
        number = defaultNumber;
    }
}

[System.Serializable]
public class TextInput : BasicInput
{
    
}