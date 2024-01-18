using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class MatchJsonCode : MonoBehaviour
{
    public Transform variableContainer;
    public TextMeshProUGUI matchNumber;

    public SettingsSaver SS;
    [SerializeField] string[] directorySeperators;
    string filePath { get => Path.Combine(Application.persistentDataPath, Path.Combine(directorySeperators)); }

    [EditorCools.Button]
    public void Save()
    {
        List<MatchInformation> savedInformation = new();

        for (int i = 0; i < variableContainer.childCount; i++)
        {
            GeneralVariable generalVariable = variableContainer.GetChild(i).GetComponent<GeneralVariable>();
            Type type = generalVariable.GetType();
            string content;

            switch (generalVariable)
            {
                case IntegerVariable integerVariable:
                    content = integerVariable.value.ToString();
                    break;

                case BooleanVariable booleanVariable:
                    content = booleanVariable.value.isOn.ToString();
                    break;

                case DropdownVariable dropdownVariable:
                    content = dropdownVariable.dropdown.options[dropdownVariable.dropdown.value].text;
                    break;

                case TextVariable textVariable:
                    content = textVariable.inputField.text;
                    break;

                default:
                    Debug.LogWarning($"{generalVariable.GetType().Name} has no functionality for saving match info");
                    continue;
            }

            savedInformation.Add(new MatchInformation(generalVariable.infoName, content));
        }

        File.WriteAllText(filePath + matchNumber.text + SS.API_int.CompHolder.Comps[SS.currentConfigDevice.competition].name +  ".json", JsonHelper.ToJson(savedInformation.ToArray()));
    }

    [Serializable]
    public class MatchInformation
    {
        public string name;
        public string content;

        public MatchInformation(string name, string content)
        {
            this.name = name;
            this.content = content;
        }
    }
}