using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class ProofOfConceptXML : MonoBehaviour
{
    public InputContainer IC = new();

    [EditorCools.Button]
    public void SaveTest()
    {
        IC.Save();
    }

    [EditorCools.Button]
    public void LoadTest()
    {
        IC.Inputs = InputContainer.Load().Inputs;
    }

    [EditorCools.Button]
    public void OpenFile()
    {
        Debug.Log(Application.dataPath + "/xmlfile.xml");
    }
}

[System.Serializable]
[XmlRoot("InputCollection")]
public class InputContainer
{
    [XmlArray("Inputs"),XmlArrayItem("Input")]
    [SerializeField] public List<Input> Inputs;

    //[EditorCools.Button]
    public void Save()
    {
        var serializer = new XmlSerializer(typeof(InputContainer));
        using(var stream = new FileStream(Application.dataPath + "/xmlfile.xml", FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    //[EditorCools.Button]
    public static InputContainer Load()
    {
        var serializer = new XmlSerializer(typeof(InputContainer));
        using(var stream = new FileStream(Application.dataPath + "/xmlfile.xml", FileMode.Open))
        {
            return serializer.Deserialize(stream) as InputContainer;
        }
    }
}

[System.Serializable]
public class Input
{
    [XmlAttribute("key")]
    public string key;
    public string value;
}
