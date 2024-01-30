using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using TMPro;
using System.Linq;

public class SettingsSaver : MonoBehaviour
{
    public TMP_Dropdown colorField;
    public TMP_Dropdown fontField;
    public int fontSize;
    public TMP_Dropdown userField;
    public TMP_InputField apiKeyField;
    public TMP_InputField yearField;
    public TMP_InputField teamField;
    public string filePath;
    public PresetJsonCode pJC;
    public API_Interface API_int;
    public TextMeshProUGUI versionDisplay;
    public string version;

    public ConfigDevice currentConfigDevice = new();

    public ConfigUser defaultConfig;

    public bool HasInternet;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/";

        if (!File.Exists(filePath + "usernames" + ".txt"))
        {
            StreamWriter writer = new StreamWriter(filePath + "usernames" + ".txt");
            writer.WriteLine("default\nDEV");
            writer.Flush();
            writer.Close();

            SaveConfigUser(defaultConfig);
        }

        if (!File.Exists(filePath + "DeviceConfig.json"))
        {
            StreamWriter writer = new StreamWriter(filePath + "DeviceConfig.json");
            writer.WriteLine(JsonUtility.ToJson(currentConfigDevice));

            writer.Flush();
            writer.Close();
        }

        LoadConfigDevice();
        API_int.ReadFromFile(true);
        API_int.AddListeners();

        versionDisplay.text = version;
        versionDisplay.gameObject.SetActive(true);
    }

    public ConfigUser CurrentConfigUser()
    {
        ConfigUser buffer = new ConfigUser();

        buffer.name = userField.options[userField.value].text;
        buffer.color = colorField.value;
        buffer.font = fontField.value;
        buffer.fontSize = fontSize;

        return buffer;
    }

    public void SaveConfigUser()
    {
        if (userField.options[userField.value].text != "default") SaveConfigUser(CurrentConfigUser());
    }

    public void SaveConfigUser(ConfigUser buffer)
    {
        StreamWriter writer = new StreamWriter(filePath + "Config_" + buffer.name + ".json");
        writer.WriteLine(JsonUtility.ToJson(buffer));

        writer.Flush();
        writer.Close();

        Debug.Log("Saved Config_" + buffer.name + ".json");
    }

    public ConfigUser ReadConfigUser()
    {
        string configPath = filePath + "Config_" + userField.options[userField.value].text + ".json";

        if (!File.Exists(configPath))
        {
            configPath = filePath + "Config_default.json";
        }

        return ReadConfigUser(configPath);
    }

    public ConfigUser ReadConfigUser(string configPath)
    {
        FileInfo file = new FileInfo(configPath);
        StreamReader reader = file.OpenText();

        ConfigUser loadedConfig = JsonUtility.FromJson<ConfigUser>(File.ReadAllText(configPath));
        reader.Close();
        Debug.Log("Read " + configPath);
        return loadedConfig;
    }

    public void LoadConfigUser()
    {
        Debug.Log("Started Loading Config");
        LoadConfigUser(ReadConfigUser());
    }

    public void LoadConfigUser(ConfigUser loadedConfig)
    {
        ConfigUser buffer = loadedConfig;
        colorField.value = buffer.color;
        fontField.value = buffer.font;
        fontSize = buffer.fontSize;

        Debug.Log("Loaded config for " + buffer.name);
    }

    public void SaveConfigDevice()
    {
        currentConfigDevice.apiKey = apiKeyField.text;
        currentConfigDevice.year = yearField.text;
        if (pJC.selectionDropdownScouter.options.Count > 0) currentConfigDevice.preset = pJC.selectionDropdownScouter.options[pJC.selectionDropdownScouter.value].text;
        currentConfigDevice.team = teamField.text;
        if (API_int.compDrop.options.Count > 0) currentConfigDevice.competition = API_int.compDrop.value;


        StreamWriter writer = new StreamWriter(filePath + "DeviceConfig.json");
        writer.WriteLine(JsonUtility.ToJson(currentConfigDevice));

        writer.Flush();
        writer.Close();

        Debug.Log("Saved DeviceConfig.json");

        API_int.ResetTeamDrop(API_int.CompHolder.Comps[API_int.compDrop.value]);
    }

    public void LoadConfigDevice()
    {
        currentConfigDevice = ReadConfigDevice();

        yearField.text = currentConfigDevice.year;
        apiKeyField.text = currentConfigDevice.apiKey;
        teamField.text = currentConfigDevice.team;
        API_int.compDrop.value = currentConfigDevice.competition;

        pJC.Load(pJC.scouterContainer, currentConfigDevice.preset);
        pJC.ReloadDropdown(0, currentConfigDevice.preset);
    }

    public ConfigDevice ReadConfigDevice()
    {
        if (!File.Exists(filePath + "DeviceConfig.json")) SaveConfigDevice();

        string configPath = filePath + "DeviceConfig.json";
        FileInfo file = new FileInfo(configPath);
        StreamReader reader = file.OpenText();

        ConfigDevice loadedConfig = JsonUtility.FromJson<ConfigDevice>(File.ReadAllText(configPath));
        reader.Close();
        Debug.Log("Read " + configPath);
        return loadedConfig;
    }

}

[System.Serializable]
public class ConfigUser
{
    public string name;
    public int color;
    public int font;
    public int fontSize;

}

[System.Serializable]
public class ConfigDevice
{
    public string apiKey;
    public string year;
    public string preset;
    public string team;
    public int competition;
}