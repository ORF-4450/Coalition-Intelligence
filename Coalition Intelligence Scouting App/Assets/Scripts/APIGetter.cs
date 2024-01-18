using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class APIGetter : MonoBehaviour
{
    [SerializeField] public SettingsSaver SS;
    private static string baseURL = "https://www.thebluealliance.com/api/v3";
    private static string authKeyName = "X-TBA-Auth-Key";
    private string authKeyValue { get => SS.currentConfigDevice.apiKey; }

    public IEnumerator GetInformation<T>(List<T> list, string link)
    {
        list.Clear();

        string result = "";

        using (UnityWebRequest request = UnityWebRequest.Get($"{baseURL}{link}"))
        {
            request.SetRequestHeader(authKeyName, authKeyValue);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                yield break;
            }
            else
            {
                result = request.downloadHandler.text;
                if (result == "[]\n") yield break;

                result = ConvertToUseableJson(result);
                list.AddRange(JsonHelper.FromJson<T>(result));
            }
        }

        Debug.Log("Finished APIGetter");
    }
    public IEnumerator GetInformation<T>(List<T> list, string prePageLink, string postPageLink)
    {
        list.Clear();

        string result = "";
        int index = 0;

        while (result != "[]\n")  
        {
            using (UnityWebRequest request = UnityWebRequest.Get($"{baseURL}{prePageLink}{index}{postPageLink}"))
            {
                request.SetRequestHeader(authKeyName, authKeyValue);

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                    yield break;
                }
                else
                {
                    result = request.downloadHandler.text;
                    if (result == "[]\n")
                    {
                        Debug.Log($"Finished at page {index}");
                        yield break;
                    }

                    result = ConvertToUseableJson(result);
                    list.AddRange(JsonHelper.FromJson<T>(result));
                    index++;
                }
            }
        }
        Debug.Log("Finished APIGetter");
    }
    
    private string ConvertToUseableJson(string input) { return $"{{\"Items\":{input.Substring(0, input.Length - 1)}}}"; }
}

///This is being used because the JsonUtility has worked weird for me sometimes, so I stopped trusting it
// Class is from https://stackoverflow.com/a/36244111

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

#region Blue Alliance API Classes
    #region API Methods
    public class APIMethodsPaged
    {
        public virtual string prePageLink() { return ""; }
        public virtual string prePageLink(int i) { return ""; }
        public virtual string postPageLink() { return ""; }
    }
    public class APIMethodsSingle
    {
        public virtual string fullLink() { return ""; }
        public virtual string fullLink(int i) { return ""; }
        public virtual string fullLink(string s) { return ""; }
    }
    #endregion

    #region list
    #region /teams/{page_num}/simple
    [Serializable]
    public class SimpleTeams : APIMethodsPaged
    {
        public string key;
        public int team_number;
        public string nickname;
        public string name;
        public string city;
        public string state_prov;
        public string country;

        public override string prePageLink() => "/teams/";
        public override string postPageLink() => "/simple";
    }
    #endregion
    #region /teams/{page_num}/keys
    [Serializable]
    public class TeamKeys : APIMethodsPaged
    {
        public string key;

        public override string prePageLink() => "/teams/";
        public override string postPageLink() => "/keys";
    }
    #endregion
    #region /teams/{year}/{page_num}/simple
    [Serializable]
    public class SimpleTeamsInYear : APIMethodsPaged
    {
        public string key;
        public int team_number;
        public string nickname;
        public string name;
        public string city;
        public string state_prov;
        public string country;

        public override string prePageLink(int year) => $"/teams/{year}/";
        public override string postPageLink() => "/keys";
    }
    #endregion
    #region /teams/{year}/{page_num}/keys
    [Serializable]
    public class TeamKeysInYear : APIMethodsPaged
    {
        public string key;

        public override string prePageLink(int year) => $"/teams/{year}/";
        public override string postPageLink() => $"/keys";
    }
    #endregion
    #region /team/{team_key}/events/{year}/statuses NOT DONE-----
    [Serializable]
    public class EventStatusesForTeamInYear : APIMethodsSingle
    {
        public AdditionalProp additionalProp1;
        public AdditionalProp additionalProp2;
        public AdditionalProp additionalProp3;
    }

    public class AdditionalProp
    {
        public Qual qual;
        public Alliance alliance;
        public Playoff playoff;
        public string alliance_status_str;
        public string playoff_status_str;
        public string overall_status_str;
        public string next_match_key;
        public string last_match_key;
    }

    public class Alliance
    {
        public string name;
        public int number;
        public Backup backup;
        public int pick;
    }

    public class Backup
    {
        public string @out;
        public string @in;
    }

    public class CurrentLevelRecord
    {
        public int losses;
        public int wins;
        public int ties;
    }

    public class Playoff
    {
        public string level;
        public CurrentLevelRecord current_level_record;
        public Record record;
        public string status;
        public int playoff_average;
    }

    public class Qual
    {
        public int num_teams;
        public Ranking ranking;
        public List<SortOrderInfo> sort_order_info;
        public string status;
    }

    public class Ranking
    {
        public int matches_played;
        public int qual_average;
        public List<int> sort_orders;
        public Record record;
        public int rank;
        public int dq;
        public string team_key;
    }

    public class Record
    {
        public int losses;
        public int wins;
        public int ties;
    }

    public class SortOrderInfo
    {
        public int precision;
        public string name;
    }


    #endregion
    #region /events/{year}/simple
    [Serializable]
    public class SimpleEventsInYear : APIMethodsSingle
    {
        public string key;
        public string name;
        public string event_code;
        public int event_type;
        public District district;
        [Serializable]
        public class District
        {
            public string abbreviation;
            public string display_name;
            public string key;
            public int year;
        }
        public string city;
        public string state_prov;
        public string country;
        public string start_date;
        public string end_date;
        public int year;

        public override string fullLink(int year) => $"/events/{year}/simple";
    }
    #endregion
    #region /events/{year}/keys
    [Serializable]
    public class EventKeysInYear : APIMethodsSingle
    {
        public string key;

        public override string fullLink(int year) => $"/events/{year}/keys";
    }

    #endregion
    #region /event/{event_key}/teams/simple
    [Serializable]
    public class SimpleTeamsInEvent : APIMethodsSingle
    {
        public string key;
        public int team_number;
        public string nickname;
        public string name;
        public string city;
        public string state_prov;
        public string country;

        public override string fullLink(string event_key) => $"/event/{event_key}/teams/simple";
    }
    #endregion
    #region /event/{event_key}/teams/keys
    [Serializable]
    public class TeamKeysInEvent : APIMethodsSingle
    {
        public string key;

        public override string fullLink(string event_key) => $"/event/{event_key}/teams/keys";
    }
    #endregion
    #region /event/{event_key}/teams/statuses NOT DONE-----------
    #endregion
    #region /district/{district_key}/events/simple
    public class SimpleEventsInDistrict : APIMethodsSingle
    {
        public string key;
        public string name;
        public string event_code;
        public int event_type;
        public District district;
        [Serializable]
        public class District
        {
            public string abbreviation;
            public string display_name;
            public string key;
            public int year;
        }
        public string city;
        public string state_prov;
        public string country;
        public string start_date;
        public string end_date;
        public int year;

        public override string fullLink(string district_key) => $"/district/{district_key}/events/simple";
    }
    #endregion
    #region /district/{district_key}/events/keys
    [Serializable]
    public class EventKeysInDistrict : APIMethodsSingle
    {
        public string key;

        public override string fullLink(string district_key) => $"/district/{district_key}/events/keys";
    }
    #endregion
    #region /district/{district_key}/teams/simple
    [Serializable]
    public class SimpleTeamsInDistrict : APIMethodsSingle
    {
        public string key;
        public int team_number;
        public string nickname;
        public string name;
        public string city;
        public string state_prov;
        public string country;

        public override string fullLink(string district_key) => $"/district/{district_key}/teams/simple";
    }
    #endregion
    #region /district/{district_key}/teams/keys
    [Serializable]
    public class TeamKeysInDistrict : APIMethodsSingle
    {
        string key;

        public override string fullLink(string district_key) => $"/district/{district_key}/teams/keys";
    }
    #endregion
    #region /district/{district_key}/rankings
    [Serializable]
    public class RankingInDistrict : APIMethodsSingle
    {
        public string team_key;
        public int rank;
        public int rookie_bonus;
        public int point_total;
        public List<EventPoint> event_points;
        [Serializable]
        public class EventPoint
        {
            public bool district_cmp;
            public int total;
            public int alliance_points;
            public int elim_points;
            public int award_points;
            public string event_key;
            public int qual_points;
        }

        public override string fullLink(string district_key) => $"/district/{district_key}/rankings";
    }
    #endregion
    #region /team/{team_key}/events/{year}/simple
    [Serializable]
    public class SimpleEventsInYearSaidTeamParticipatedIn : APIMethodsSingle
    {
        public string key;
        public string name;
        public string eventCode;
        public int eventType;
        public District district;
        [Serializable]
        public class District
        {
            public string abbreviation;
            public string displayName;
            public string key;
            public int year;
        }
        public string city;
        public string stateProv;
        public string country;
        public string startDate;
        public string endDate;
        public int year;
    }
    #endregion
    #endregion
    #endregion