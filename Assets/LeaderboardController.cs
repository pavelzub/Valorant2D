
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
    public static string URL = "https://mazerix-e6c90-default-rtdb.europe-west1.firebasedatabase.app/.json";

    private int score;
    private string username;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(DeleteRequest(URL));
        // эту хуйню удалить, чисто тест пока
        Player user = new Player();
        user.name = "Joe";
        user.score = 11;
        string json = JsonUtility.ToJson(user);
        //StartCoroutine(PostRequest(URL, json.ToString()));

    }

    // вызывается после проигрыша и после нажатия кнопки ОК в окне ввода юзернейма
    public void SetUserStatistic(string username, int score)
    {
        this.username = username;
        this.score = score;
    }

    IEnumerator PostRequest(string url, string json)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Отправляем запрос и ждем ответа
        yield return request.SendWebRequest();

        // Проверяем, произошла ли ошибка при отправке запроса
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            yield return StartCoroutine(GetRequest(URL));
            // Выводим ответ в консоль
            Debug.Log(request.downloadHandler.text);
        }
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Отправляем запрос и ждем ответа
            yield return webRequest.SendWebRequest();

            // Проверяем, произошла ли ошибка при отправке запроса
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                // Выводим ответ в консоль
                var players = ParseResponse(webRequest.downloadHandler.text);
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

        // Отправляем запрос и ждем ответа
        yield return request.SendWebRequest();

        // Проверяем, произошла ли ошибка при отправке запроса
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // Выводим ответ в консоль
            Debug.Log("All data deleted successfully.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
