using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class WebBehaviours : MonoBehaviour
{
    private string urlToOpen = "https://classroom.google.com/";

    private string apiURL = "http://worldtimeapi.org/api/timezone/Etc/GMT";
    private string apiURL2 = "http://api.geonames.org/timezone?lat=0&lng=0&username=demo\r\n";
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Application.OpenURL(urlToOpen);
        }
        else if (Input.GetKeyDown(KeyCode.O)) 
        {
            StartCoroutine(GetTimeZone());
        }
        else if (Input.GetKeyDown(KeyCode.P)) 
        { 

        }
    }

    IEnumerator GetTimeZone()
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(apiURL2);

        yield return unityWebRequest.SendWebRequest();  

        if(unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonData = unityWebRequest.downloadHandler.text;

            Debug.Log(jsonData);

            var data = JsonUtility.FromJson<WorldTimeResult>(jsonData);

            Debug.Log(data.datatime);
            Debug.Log(data.timezone);
        }
    }

    [Serializable]
    public class WorldTimeResult
    {
        public string datatime;
        public string timezone;
    }
}
