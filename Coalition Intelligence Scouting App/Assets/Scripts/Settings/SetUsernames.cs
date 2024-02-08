using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

using System.Linq;

public class SetUsernames : MonoBehaviour
{
    TMP_Dropdown dropdown;

    public SettingsSaver SS;


    void Start()
    {
        reloadUsernames();
    }


    public void OnDisable()
    {
        SS.SaveConfigUser();
        SS.SaveConfigDevice();
    }

    public void openUsernameFile(string file)
    {
        Application.OpenURL(SS.filePath + file);
    }

    public void reloadUsernames()
    {
        List<TMP_Dropdown.OptionData> options = new();

        dropdown = gameObject.GetComponent<TMP_Dropdown>();

        FileInfo file = new FileInfo (SS.filePath + "usernames" + ".txt");
        StreamReader reader = file.OpenText();

        string[] linesArray = File.ReadAllLines(SS.filePath + "usernames" + ".txt");
        List<string> lines = linesArray.ToList();

        lines.Remove("default");
        lines.Remove("DEV");

        lines.Sort();

        lines.Insert(0, "default");
        lines.Insert(1, "DEV");

        

        reader.Close();

        foreach (string name in lines)
        {
            TMP_Dropdown.OptionData buffer = new();
            buffer.text = name;
            options.Add(buffer);
        }
        
        dropdown.options = options;

        gameObject.GetComponent<EnableOnValue>().Check();
    }
}
