using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DBConnector : MonoBehaviour
{
    private static string URL = "https://mazerix-e6c90-default-rtdb.europe-west1.firebasedatabase.app/.json";

    [Serializable]
    public class Player {
        public string name;
        public int score;
    }

    [Serializable]
    public class PlayersData {
        public Dictionary<string, Player> players;
    }

    private IEnumerator PostRequest(string url, string json) {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.LogError(request.error);
        }
        else {
            //yield return StartCoroutine(GetRequest(URL));
            Debug.Log(request.downloadHandler.text);
        }
    }


    public void SendScore(string name, int score) {
        Player user = new Player();
        user.name = name;
        user.score = score;
        string json = JsonUtility.ToJson(user);
        StartCoroutine(PostRequest(URL, json.ToString()));
    }

    public void GetScores() { 
    
    }
}
