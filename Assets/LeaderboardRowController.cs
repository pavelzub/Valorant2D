using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardRowController : MonoBehaviour
{
    public GameObject score;
    public GameObject playerName;

    // Start is called before the first frame update

    public void SetupPlayerInfo(string name, int score)
    {
        playerName.GetComponent<TMP_Text>().text = name;
        this.score.GetComponent<TMP_Text>().text = "Score: " + score.ToString();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
