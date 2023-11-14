using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class CSVfileCreatorProofOfConcept : MonoBehaviour
{
    string filePath;
    [SerializeField] List<string> dataKeys;
    [SerializeField] List<string> theData;
    // Start is called before the first frame update
    void Awake()
    {
        filePath = Application.persistentDataPath + "/" + "thedatas.csv";
    }

[EditorCools.Button]
    void test()
    {
        WriteData(filePath, theData);
    }

    void WriteData(string path, List<string> dataList)
    {
        StreamWriter writer = new StreamWriter(path, true);
        string line = "";
        foreach(string data in dataList)
        {
            if (line != "")
            {
                line = line + "," + data;
            } else {
                line = data;
            }
        }

        writer.WriteLine(line);
        writer.Flush();
        writer.Close();
    }

    [EditorCools.Button]
    void testTwo()
    {
        DeleteData(filePath, dataKeys);
    }
    void DeleteData(string path, List<string> keys)
    {
        StreamWriter writer = new StreamWriter(path);
        Debug.Log(path);
        string line = "";
        foreach(string key in keys)
        {
            if (line != "")
            {
                line = line + "," + key;
            } else {
                line = key;
            }
        }
        writer.WriteLine(line);
        writer.Flush();
        writer.Close();
    }
}