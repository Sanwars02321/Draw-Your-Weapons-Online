using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LeaderboardUI : MonoBehaviour
{
    public TextMeshProUGUI playerNames, playerScores;
    public Button TopWinners, TopKillers;
    // Start is called before the first frame update
    void Awake()
    {
        playerNames.enabled = false;
        playerScores.enabled = false;

        TopWinners.onClick.AddListener(()=> GetLeaderBoard(32136));
        TopKillers.onClick.AddListener(()=> GetLeaderBoard(32135));
    }

    // Update is called once per frame
    void GetLeaderBoard(int leaderboardID)
    {
        playerNames.enabled = true;
        playerScores.enabled = true;
        LootLockerManager.Instance.FetchTopHighScore(playerNames, playerScores, leaderboardID);
    }
}
