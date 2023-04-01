
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;
using System.Text;

public class LeaderboardController : MonoBehaviour
{
    public static string URL = "https://mazerix-e6c90-default-rtdb.europe-west1.firebasedatabase.app/.json";
    public GameObject rawPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
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
                // ������� ����� � �������
                Debug.Log(webRequest.downloadHandler.text);
                rawPrefab.GetComponent<TMP_Text>().text = webRequest.downloadHandler.text;
            }
        }
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
            // ������� ����� � �������
            Debug.Log(request.downloadHandler.text);
        }
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
