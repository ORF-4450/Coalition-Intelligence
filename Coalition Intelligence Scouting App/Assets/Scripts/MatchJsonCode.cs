using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MatchJsonCode : MonoBehaviour
{
    public Transform variableContainer;
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

        File.WriteAllText(filePath, JsonHelper.ToJson(savedInformation.ToArray()));
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