using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class PresetJsonCode : MonoBehaviour
{
    public Transform variableContainer;
    [SerializeField] string[] directorySeperators;
    string filePath { get => Path.Combine(Application.persistentDataPath, Path.Combine(directorySeperators)); }
    [Space][Space][Header("Variable Prefabs")]
    public GameObject integerPrefab;
    public GameObject booleanPrefab;
    public GameObject dropdownPrefab;
    public GameObject textPrefab;

    // Could convert these to static by inputting variableContainer, filePath
    [EditorCools.Button]
    public void Save()
    {
        List<VariableGeneralInformation> savedInformation = new();

        for (int i = 0; i < variableContainer.childCount; i++)
        {
            GeneralVariable generalVariable = variableContainer.GetChild(i).GetComponent<GeneralVariable>();
            Type type = generalVariable.GetType();
            string content;

            switch (generalVariable)
            {
                case IntegerVariable integerVariable:
                    SavedIntegerInfo savedIntegerInfo = new SavedIntegerInfo(integerVariable.minimumValueExists,
                                                                             integerVariable.minimumValue,
                                                                             integerVariable.maximumValueExists,
                                                                             integerVariable.maximumValue);
                    content = JsonHelper.ToJson(new SavedIntegerInfo[] { savedIntegerInfo });
                    break;

                case BooleanVariable:
                    content = "false";
                    break;

                case DropdownVariable:
                    content = JsonHelper.ToJson((generalVariable as DropdownVariable).dropdown.options.ToArray());
                    break;

                case TextVariable textVariable:
                    content = "";
                    break;

                default:
                    Debug.LogWarning($"{generalVariable.GetType().Name} has no functionality for saving presets");
                    continue;
            }

            savedInformation.Add(new VariableGeneralInformation(type, generalVariable.infoName, content));
        }

        File.WriteAllText(filePath, JsonHelper.ToJson(savedInformation.ToArray()));
    }
    // Load would also need a PresetCode instance or have prefab GameObjects to be static
    [EditorCools.Button]
    public void Load()
    {
        string jsonContent = "{}";
        using (StreamReader reader = new StreamReader(filePath))
        {
            jsonContent = reader.ReadToEnd();
        }
        VariableGeneralInformation[] savedInformation = JsonHelper.FromJson<VariableGeneralInformation>(jsonContent);
        List<GeneralVariable> variables = new();

        for (int i = 0; i < savedInformation.Length; i++)
        {
            GameObject prefab;
            Debug.Log(savedInformation[i].type);

            switch (savedInformation[i].type.ToString())
            {

                case "IntegerVariable":
                    prefab = integerPrefab;
                    IntegerVariable integerVariable = prefab.GetComponent<IntegerVariable>();
                    SavedIntegerInfo savedIntegerInfo =
                        JsonHelper.FromJson<SavedIntegerInfo>(savedInformation[i].content)[0];
                    integerVariable.minimumValueExists = savedIntegerInfo.minimumValueExists;
                    integerVariable.minimumValue = savedIntegerInfo.minimumValue;
                    integerVariable.maximumValueExists = savedIntegerInfo.maximumValueExists;
                    integerVariable.maximumValue = savedIntegerInfo.maximumValue;
                    variables.Add(integerVariable);
                    break;

                case "BooleanVariable":
                    prefab = booleanPrefab;
                    BooleanVariable booleanVariable = prefab.GetComponent<BooleanVariable>();
                    // Could just be set to false, but if anyone decides to change how default values are calculated, we only need to
                    // change in save and not in both
                    booleanVariable.value.isOn = bool.Parse(savedInformation[i].content);
                    variables.Add(booleanVariable);
                    break;

                case "DropdownVariable":
                    prefab = dropdownPrefab;
                    DropdownVariable dropdownVariable = prefab.GetComponent<DropdownVariable>();
                    dropdownVariable.dropdown.options = new List<TMP_Dropdown.OptionData>(JsonHelper.FromJson<TMP_Dropdown.OptionData>(savedInformation[i].content));
                    variables.Add(dropdownVariable);
                    break;

                case "TextVariable":
                    prefab = textPrefab;
                    TextVariable textVariable = prefab.GetComponent<TextVariable>();
                    textVariable.inputField.text = savedInformation[i].content;
                    break;

                default:
                    Debug.LogWarning($"{savedInformation[i].name} has no functionality for saving presets");
                    continue;
            }

            Instantiate(prefab, variableContainer);
        }
    }


    [Serializable]
    public class VariableGeneralInformation
    {
        // Type cannot be stored in a json
        [SerializeField] private string _type;
        public Type type { get => Type.GetType(_type); set => _type = value.ToString(); }
        public string name;
        public string content;

        public VariableGeneralInformation(Type type, string name, string content)
        {
            this.type = type;
            this.name = name;
            this.content = content;
        }
    }

    [Serializable]
    public class SavedIntegerInfo
    {
        public bool minimumValueExists;
        public int minimumValue;
        public bool maximumValueExists;
        public int maximumValue;

        public SavedIntegerInfo(bool minimumValueExists, int minimumValue, bool maximumValueExists, int maximumValue)
        {
            this.minimumValueExists = minimumValueExists;
            this.minimumValue = minimumValue;
            this.maximumValueExists = maximumValueExists;
            this.maximumValue = maximumValue;
        }
    }
}