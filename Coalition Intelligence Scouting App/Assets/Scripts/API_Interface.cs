using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using TMPro;
using UnityEngine.UI;
public class API_Interface : MonoBehaviour
{
    [SerializeField] SettingsSaver SS { get => API.SS;}
    [SerializeField] public APIGetter API;
    public ConfigDevice config { get => SS.currentConfigDevice; }
    private string fileName { get => SS.filePath + "CompetitionInfo.json";}
    [SerializeField] public CompetitionList CompHolder = new();
    [SerializeField] public TMP_Dropdown teamDrop;
    [SerializeField] public TMP_Dropdown compDrop;
    [SerializeField] public List<SimpleTeamsInEvent> debugTeams;

    [SerializeField] public Button resetTBAInfo;

    [SerializeField] public SimpleTeamsInEvent defaultTeam = new();

    public void AddListeners()
    {
        //compDrop.onValueChanged.AddListener((int val) => ResetTeamDrop(CompHolder.Comps[val]));
        resetTBAInfo.onClick.AddListener(() => SyncWithTBA());
    }

    [EditorCools.Button]
    public void SyncWithTBA()
    {
        StartCoroutine(SetCompHolder(true));
        // ReadFromFile(true);
    }

    [EditorCools.Button]
    public void ReadFromFile()
    {
        ReadFromFile(false);
    }

    public void ReadFromFile(bool setCompToFile)
    {
        CompHolder.Comps.Clear();
        if (File.Exists(fileName))
        {
            FileInfo file = new FileInfo(fileName);
            StreamReader reader = file.OpenText();

            CompHolder = JsonUtility.FromJson<CompetitionList>(File.ReadAllText(fileName));
            reader.Close();
        } else {
            CompHolder = new CompetitionList();
            Debug.LogError("TBA File Does Not Exist at\n" + fileName + "\nPlease Sync with TBA.");
        }

        CompHolder.Comps.Add(new Competition("DEBUG_Comp", "DEBUG", debugTeams));

        ResetDrops(setCompToFile);
    }

    [EditorCools.Button]
    public void ResetDrops()
    {
        ResetCompDrop();
        ResetTeamDrop(CompHolder.Comps[compDrop.value]);
    }

    public void ResetDrops(bool setCompToFile)
    {
        if (!setCompToFile)
        {
            ResetDrops();
        } else {
            ResetCompDrop();
            ResetTeamDrop(CompHolder.Comps[SS.currentConfigDevice.competition]);
        }
    }

    public void ResetCompDrop()
    {
        compDrop.value = 0;
        compDrop.ClearOptions();
        List<TMP_Dropdown.OptionData> buffer = new();
        foreach (Competition comp in CompHolder.Comps)
        {
            buffer.Add(new TMP_Dropdown.OptionData(comp.name));
        }
        compDrop.options = buffer;

        compDrop.RefreshShownValue();
    }

    public void ResetTeamDrop()
    {
        teamDrop.value = 0;
        teamDrop.ClearOptions();
        teamDrop.RefreshShownValue();
        Debug.Log("Reset Team Drop");
    }

    public void ResetTeamDrop(Competition comp)
    {
        teamDrop.value = 0;
        List<TMP_Dropdown.OptionData> buffer = new();
        Debug.Log("Resetting team dropdown with " + comp.name);

        foreach (SimpleTeamsInEvent team in comp.teams)
        {
            buffer.Add(new TMP_Dropdown.OptionData(team.nickname + " - " + team.team_number));
            Debug.Log("Added " + team.nickname + " - " + team.team_number + " to team dropdown");
        }
        teamDrop.options = buffer;
        teamDrop.RefreshShownValue();
    }


    private IEnumerator SetCompHolder(bool saveToFile)
    {
        CompHolder.Comps.Clear();
        Debug.Log("Started Syncing with TBA");

        List<SimpleEventsInYearSaidTeamParticipatedIn> buffer = new();
        yield return StartCoroutine(API.GetInformation<SimpleEventsInYearSaidTeamParticipatedIn>(buffer, "/team/frc" + config.team + "/events/" + config.year + "/simple"));
        Debug.Log("Finished Syncing with TBA");

        CompetitionList compBuffer = new();

        foreach (SimpleEventsInYearSaidTeamParticipatedIn comp in buffer)
        {
            List<SimpleTeamsInEvent> teamsBuffer = new();
            yield return StartCoroutine(API.GetInformation<SimpleTeamsInEvent>(teamsBuffer, "/event/" + comp.key + "/teams/simple"));
            teamsBuffer.Insert(0, defaultTeam);
            
            compBuffer.Comps.Add(new Competition(comp.name, comp.key, teamsBuffer));
        }


        if (saveToFile)
        {
            StreamWriter writer = new StreamWriter(fileName);
            writer.WriteLine(JsonUtility.ToJson(compBuffer));

            writer.Flush();
            writer.Close();
            Debug.Log("Wrote comp data to file.");
        }
        
        ReadFromFile(true);
    }
}

[System.Serializable]
public class CompetitionList
{
    public List<Competition> Comps = new();
}

[System.Serializable]
public class Competition
{
    public string name;
    public string key;
    public List<SimpleTeamsInEvent> teams = new();

    public Competition(string nameInput, string keyInput, List<SimpleTeamsInEvent> teamsInput)
    {
        name = nameInput;
        key = keyInput;
        teams = teamsInput;
    }
}