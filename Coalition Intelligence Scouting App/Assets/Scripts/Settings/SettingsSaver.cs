using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using TMPro;

public class SettingsSaver : MonoBehaviour
{
    public TMP_Dropdown colorField;
    public TMP_Dropdown fontField;
    public int fontSize;
    public TMP_Dropdown userField;
    public TMP_InputField apiKeyField;
    public TMP_InputField yearField;
    public string filePath;
    public PresetJsonCode pJC;

    public ConfigDevice currentConfigDevice = new();

    public ConfigUser defaultConfig;

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

        LoadConfigDevice();
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
        currentConfigDevice.preset = pJC.selectionDropdownScouter.value;


        StreamWriter writer = new StreamWriter(filePath + "DeviceConfig.json");
        writer.WriteLine(JsonUtility.ToJson(currentConfigDevice));

        writer.Flush();
        writer.Close();

        Debug.Log("Saved DeviceConfig.json");
    }

    public void LoadConfigDevice()
    {
        currentConfigDevice = ReadConfigDevice();

        yearField.text = currentConfigDevice.year;
        apiKeyField.text = currentConfigDevice.apiKey;

        pJC.selectionDropdownScouter.value = currentConfigDevice.preset;

        pJC.Load(pJC.scouterContainer, pJC.selectionDropdownScouter.options[currentConfigDevice.preset].text);
        pJC.ReloadDropdown();
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

public class ConfigDevice
{
    public string apiKey;
    public string year;
    public int preset;
}