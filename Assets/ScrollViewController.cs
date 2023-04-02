using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour
{
    public GameObject content;
    public GameObject playerResultPrefab;

    public void AddContent(List<Player> players)
    {
        players.Sort((p1, p2) => p2.score.CompareTo(p1.score));
        foreach (Player player in players) {
            GameObject itemInScrollView = Instantiate(playerResultPrefab, content.transform);
            LeaderboardRowController leaderboardRowController = itemInScrollView.GetComponent<LeaderboardRowController>();
            leaderboardRowController.SetupPlayerInfo(player.name, player.score);
            //itemInScrollView.transform.SetParent(scrollView.content);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
