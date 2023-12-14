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
        dropdown = gameObject.GetComponent<TMP_Dropdown>();

        FileInfo file = new FileInfo (SS.filePath + "usernames" + ".txt");
        StreamReader reader = file.OpenText();

        string[] lines = File.ReadAllLines(SS.filePath + "usernames" + ".txt");

        reader.Close();

        List<TMP_Dropdown.OptionData> options = new();

        foreach (string name in lines.ToList())
        {
            TMP_Dropdown.OptionData buffer = new();
            buffer.text = name;
            options.Add(buffer);
        }
        
        dropdown.options = options;
    }

    public void OnDisable()
    {
        SS.SaveConfigUser();
        SS.SaveConfigDevice();
    }
}
