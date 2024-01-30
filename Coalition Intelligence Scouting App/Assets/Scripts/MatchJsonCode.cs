using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class MatchJsonCode : MonoBehaviour
{
    public Transform variableContainer;
    public TMP_InputField matchNumber;

    public MenuSelector menuSelector;
    public MenuSwapper MS { get => menuSelector.menuSwapper; }
    
    
    public string team { get => SS.API_int.teamDrop.options[SS.API_int.teamDrop.value].text.Split(' ')[0]; }

    public SettingsSaver SS;
    [SerializeField] string[] directorySeperators;
    public string filePath { get => Path.Combine(Application.persistentDataPath, Path.Combine(directorySeperators)); }

    public void Awake()
    {
        if(!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
    }

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
                    content = dropdownVariable.dropdown.value.ToString();
                    break;

                case TextVariable textVariable:
                    content = textVariable.inputField.text;
                    break;

                default:
                    Debug.LogWarning($"{generalVariable.GetType().Name} has no functionality for saving match info");
                    continue;
            }

            savedInformation.Add(new MatchInformation(generalVariable.infoName, generalVariable.GetType().Name, content, generalVariable.infoKey));
        }

        string compFilePath = Path.Combine(filePath, SS.API_int.CompHolder.Comps[SS.currentConfigDevice.competition].name + "_" + SS.currentConfigDevice.year) + "/";
        if(!Directory.Exists(compFilePath)) Directory.CreateDirectory(compFilePath);

        Match savedMatch = new Match(matchNumber.text, team, savedInformation);

        File.WriteAllText(compFilePath + matchNumber.text + "_" + team +  ".json", savedMatch.SaveToString());

        List<string> keys = new();
        keys.Add("roundNum");
        keys.Add("teamNum");
        foreach (MatchInformation data in savedMatch.data)
        {
            keys.Add(data.key);
        }

        string keyPath = compFilePath + "keyConfig.csv";
        File.WriteAllText(keyPath, String.Join(',',keys));

        FormatData(compFilePath,compFilePath + "formattedData.csv",keyPath);

        if (menuSelector != null)
        {
            menuSelector.ChangeMenu(MS.defaultMenu);
        } else {
            MS.ChangeMenu(MS.defaultMenu);
        }

        SS.API_int.SetTeamToDefault();
    }

    public void FormatData(string compFilePath, string fileName, string keyPath)
    {
        FileInfo keyConfig = new FileInfo(keyPath);
        StreamReader readerKeys = keyConfig.OpenText();
        File.WriteAllText(fileName, File.ReadAllText(keyPath) + "\n"); //Writes the keys at the top of the file
        readerKeys.Close();


        DirectoryInfo d = new DirectoryInfo(compFilePath);

        foreach (FileInfo file in d.GetFiles("*.json"))
        {
            StreamReader reader = file.OpenText();
            Match match = JsonUtility.FromJson<Match>(File.ReadAllText(file.FullName));
            reader.Close();
            string csv = match.ConvertToCSV();

            StreamWriter writer = new StreamWriter(fileName, true);
            writer.WriteLine(csv);
            writer.Flush();
            writer.Close();
        }
    }

    [Serializable]
    public class MatchInformation
    {
        public string name;
        public string type;
        public string content;
        public string key;

        public MatchInformation(string name, string type, string content, string key)
        {
            this.name = name;
            this.type = type;
            this.content = content;
            this.key = key;
        }
    }

    [Serializable]
    public class Match
    {
        public string roundNum;
        public string teamNum;
        public List<MatchInformation> data;

        public Match(string roundNum, string teamNum, List<MatchInformation> data)
        {
            this.roundNum = roundNum;
            this.teamNum = teamNum;
            this.data = data;
        }

        public string SaveToString()
        {
            return JsonUtility.ToJson(this, true);
        }

        public string ConvertToCSV()
        {
            List<string> values = new();
            values.Add(roundNum);
            values.Add(teamNum);
            foreach (MatchInformation value in data)
            {
                values.Add(value.content);
            }
            return String.Join(',',values);
        }
    }
}