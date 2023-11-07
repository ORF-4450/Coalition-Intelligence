using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DirectBlueAllianceInfo : MonoBehaviour
{
    private static string baseURL = "https://www.thebluealliance.com/api/v3";
    private static string authKeyName = "X-TBA-Auth-Key";
    private static string authKeyValue = "";

    private IEnumerator GetInformation<T>(List<T> list, string link)
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
    }
    private IEnumerator GetInformation<T>(List<T> list, string prePageLink, string postPageLink)
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