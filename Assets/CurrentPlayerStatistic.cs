using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentPlayerStatistic : MonoBehaviour
{
    public GameObject name;
    public GameObject score;
    public void WritePlayerStatictic(string playerName, int score)
    {
        name.GetComponent<TMP_Text>().text = playerName;
        this.score.GetComponent<TMP_Text>().text = "Score: " + score.ToString();
    }


}
