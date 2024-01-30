using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresetJsonCode : MonoBehaviour
{
    public Transform editorContainer;
    public Transform scouterContainer;
    public TMP_Dropdown selectionDropdownEditor;
    public TMP_Dropdown selectionDropdownScouter;
    public TMP_InputField fileNameContainerEditor;
    [SerializeField] public Button saveJson;
    [SerializeField] public Button loadJsonEditor;
    [SerializeField] public Button loadJsonScouter;
    [SerializeField] string[] directorySeperators;
    string filePath { get => Path.Combine(Application.persistentDataPath, Path.Combine(directorySeperators)); }
    [Space][Space][Header("Variable Prefabs")]
    public GameObject integerPrefab;
    public GameObject booleanPrefab;
    public GameObject dropdownPrefab;
    public GameObject textPrefab;

    protected virtual void Awake()
    {
        if(!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

        saveJson.onClick.AddListener(() => Save(fileNameContainerEditor.text + ".json"));
        loadJsonEditor.onClick.AddListener(() => Load(editorContainer, selectionDropdownEditor.options[selectionDropdownEditor.value].text));
        loadJsonScouter.onClick.AddListener(() => Load(scouterContainer, selectionDropdownScouter.options[selectionDropdownScouter.value].text));
    }

    [EditorCools.Button]
    public void ReloadDropdown()
    {
        ReloadDropdown(0);
    }

    public void ReloadDropdown(int defaultValue)
    {
        ReloadDropdown(defaultValue,"");
    }

    public void ReloadDropdown(int defaultValue, string savedPreset)
    {
        selectionDropdownEditor.ClearOptions();
        selectionDropdownScouter.ClearOptions();

        selectionDropdownScouter.value = defaultValue;

        DirectoryInfo dir = new DirectoryInfo(filePath);
        FileInfo[] info = dir.GetFiles("*.json");
        foreach (FileInfo file in info)
        {
            selectionDropdownEditor.options.Add(new TMP_Dropdown.OptionData(file.Name));
            selectionDropdownScouter.options.Add(new TMP_Dropdown.OptionData(file.Name));

            if (file.Name == savedPreset) selectionDropdownScouter.value = selectionDropdownScouter.options.Count - 1;
        }

        selectionDropdownEditor.value = 0;
        selectionDropdownEditor.RefreshShownValue();

        selectionDropdownScouter.RefreshShownValue();
    }

    // Could convert these to static by inputting variableContainer, filePath
    [EditorCools.Button]
    public void Save(string fileName)
    {
        List<VariableGeneralInformation> savedInformation = new();

        for (int i = 0; i < editorContainer.childCount; i++)
        {
            GeneralVariable generalVariable = editorContainer.GetChild(i).GetComponent<GeneralVariable>();
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

            savedInformation.Add(new VariableGeneralInformation(type, generalVariable.infoName, content, generalVariable.infoKey));
        }

        File.WriteAllText(Path.Combine(filePath, fileName), JsonHelper.ToJson(savedInformation.ToArray(), true));

        ReloadDropdown();
    }

    public void LoadScouter()
    {
        Load(scouterContainer, selectionDropdownScouter.options[selectionDropdownScouter.value].text);   
    }

    // Load would also need a PresetCode instance or have prefab GameObjects to be static
    [EditorCools.Button]
    public void Load(Transform destination, string fileName)
    {
        if (File.Exists(Path.Combine(filePath, fileName)))
        {
            foreach(Transform child in destination)
            {
                Destroy(child.gameObject);
            }
            
            string jsonContent = "{}";
            using (StreamReader reader = new StreamReader(Path.Combine(filePath, fileName)))
            {
                jsonContent = reader.ReadToEnd();
            }
            VariableGeneralInformation[] savedInformation = JsonHelper.FromJson<VariableGeneralInformation>(jsonContent);
            List<GeneralVariable> variables = new();

            for (int i = 0; i < savedInformation.Length; i++)
            {
                GameObject prefab;
                Debug.Log(savedInformation[i].type);

                GeneralVariable variable;

                switch (savedInformation[i].type.ToString())
                {

                    case "IntegerVariable":
                        prefab = integerPrefab;
                        IntegerVariable integerVariable = prefab.GetComponent<IntegerVariable>();
                        variable = integerVariable;
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
                        variable = booleanVariable;
                        // Could just be set to false, but if anyone decides to change how default values are calculated, we only need to
                        // change in save and not in both
                        booleanVariable.value.isOn = bool.Parse(savedInformation[i].content);
                        variables.Add(booleanVariable);
                        break;

                    case "DropdownVariable":
                        prefab = dropdownPrefab;
                        DropdownVariable dropdownVariable = prefab.GetComponent<DropdownVariable>();
                        variable = dropdownVariable;
                        dropdownVariable.dropdown.options = new List<TMP_Dropdown.OptionData>(JsonHelper.FromJson<TMP_Dropdown.OptionData>(savedInformation[i].content));
                        variables.Add(dropdownVariable);
                        break;

                    case "TextVariable":
                        prefab = textPrefab;
                        TextVariable textVariable = prefab.GetComponent<TextVariable>();
                        variable = textVariable;
                        textVariable.inputField.text = savedInformation[i].content;
                        break;

                    default:
                        Debug.LogWarning($"{savedInformation[i].name} has no functionality for saving presets");
                        continue;
                }

                variable.infoName = savedInformation[i].name;
                variable.infoKey = savedInformation[i].key;
                Instantiate(prefab, destination);
            }
        } else {
            Debug.Log("No preset file found for " + fileName);
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
        public string key;

        public VariableGeneralInformation(Type type, string name, string content, string key)
        {
            this.type = type;
            this.name = name;
            this.content = content;
            this.key = key;
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