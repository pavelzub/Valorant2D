
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;

[Serializable]
public class Player
{
    public string name;
    public int score;
}

[Serializable]
public class PlayersData
{
    public Dictionary<string, Player> players;
}

public class LeaderboardController : MonoBehaviour
{
    public GameObject currentPlayerResult;
    public ScrollViewController scrollViewController;
    public static string currentPlayerName;
    public static int currentPlayerScore;

    public static string URL = "https://mazerix-e6c90-default-rtdb.europe-west1.firebasedatabase.app/.json";


    public void SetCurrentPlayerResult()
    {
        CurrentPlayerStatistic currentPlayerStatistic = currentPlayerResult.GetComponent<CurrentPlayerStatistic>();
        currentPlayerStatistic.WritePlayerStatictic(currentPlayerName, currentPlayerScore);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentPlayerResult();
        StartCoroutine(GetRequest(URL));

    }

    IEnumerator PostRequest(string url, string json)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // ���������� ������ � ���� ������
        yield return request.SendWebRequest();

        // ���������, ��������� �� ������ ��� �������� �������
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            yield return StartCoroutine(GetRequest(URL));
            // ������� ����� � �������
            Debug.Log(request.downloadHandler.text);
        }
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // ���������� ������ � ���� ������
            yield return webRequest.SendWebRequest();

            // ���������, ��������� �� ������ ��� �������� �������
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                // Get list of players from DB
                var players = ParseResponse(webRequest.downloadHandler.text);
                scrollViewController.AddContent(players);
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }

    List<Player> ParseResponse(string jsonString)
    {
        PlayersData players = new PlayersData();
        players.players = JsonConvert.DeserializeObject<Dictionary<string, Player>>(jsonString);
        List<Player> playersList = new List<Player>();
        foreach (KeyValuePair<string, Player> player in players.players)
        {
            Player p = new Player();
            p.name = player.Value.name;
            p.score = player.Value.score;
            playersList.Add(p);
        }
        return playersList;
    }

    IEnumerator DeleteRequest(string uri)
    {
        var request = new UnityWebRequest(uri, "DELETE");

        // ���������� ������ � ���� ������
        yield return request.SendWebRequest();

        // ���������, ��������� �� ������ ��� �������� �������
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // ������� ����� � �������
            Debug.Log("All data deleted successfully.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
