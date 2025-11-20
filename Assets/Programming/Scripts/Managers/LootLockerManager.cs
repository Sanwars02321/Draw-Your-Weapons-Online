using UnityEngine;
using LootLocker.Requests;
using System.Collections.Generic;
using TMPro;

public class LootLockerManager : MonoBehaviour
{
    public static LootLockerManager Instance;
    private bool sessionActive = false;

    // Stats acumuladas persistentes
    private int totalKillsAllTime = 0;
    private int totalMatchesAllTime = 0;
    private int totalWinsAllTime = 0;
    private int currentKillstreak = 0;
    private int totalRoundsWon = 0;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPersistentStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartGuestSession();
    }

    private void LoadPersistentStats()
    {
        // Cargar stats guardadas localmente
        totalKillsAllTime = PlayerPrefs.GetInt("LootLocker_TotalKills", 0);
        totalMatchesAllTime = PlayerPrefs.GetInt("LootLocker_TotalMatches", 0);
        totalWinsAllTime = PlayerPrefs.GetInt("LootLocker_TotalWins", 0);
        currentKillstreak = PlayerPrefs.GetInt("LootLocker_CurrentKS", 0);
        totalRoundsWon = PlayerPrefs.GetInt("LootLocker_TotalRoundsWon", 0);



        Debug.Log($"Stats cargadas - Kills: {totalKillsAllTime}, Matches: {totalMatchesAllTime}, Wins: {totalWinsAllTime}");
    }

    private void SavePersistentStats()
    {
        PlayerPrefs.SetInt("LootLocker_TotalKills", totalKillsAllTime);
        PlayerPrefs.SetInt("LootLocker_TotalMatches", totalMatchesAllTime);
        PlayerPrefs.SetInt("LootLocker_TotalWins", totalWinsAllTime);
        PlayerPrefs.SetInt("LootLocker_TotalRoundsWon", totalRoundsWon);
        PlayerPrefs.Save();
    }

    private void StartGuestSession()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                sessionActive = true;
                Debug.Log("Sesión LootLocker iniciada");
            }
            else
            {
                Debug.LogError("Error al iniciar sesión: " + response.errorData.message);
            }
        });
    }

    public void SendAllStats(PlayerStats matchStats)
    {
        if (!sessionActive)
        {
            Debug.LogError("No hay sesión activa en LootLocker");
            return;
        }

        totalKillsAllTime += matchStats.currentMatchKills;
        totalMatchesAllTime++;
        if (matchStats.totalWins > 0) totalWinsAllTime++;

        SavePersistentStats();

        SubmitScore("total_kills", totalKillsAllTime);
        SubmitScore("total_matches", totalMatchesAllTime);
        SubmitScore("total_wins", totalWinsAllTime);
        SubmitScore("best_killstreak", matchStats.bestKillstreak);
        SubmitScore("rounds_won", matchStats.totalRoundsWon);
    }

    private void SubmitScore(string leaderboardKey, int score)
    {
        LootLockerSDKManager.SubmitScore("", score, leaderboardKey, (response) =>
        {
            if (response.success)
            {
                Debug.Log($"Score enviado a {leaderboardKey}: {score}");
            }
            else
            {
                Debug.LogError($"Error enviando a {leaderboardKey}: " + response.errorData.message);
            }
        });
    }

    public void FetchTopHighScore(TextMeshProUGUI playerNames, TextMeshProUGUI playerScore, int leaderboardID)
    {
        LootLockerSDKManager.GetScoreList(leaderboardID.ToString(), 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";

                var members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                    playerNames.text = tempPlayerNames;
                    playerScore.text = tempPlayerScores;
                }  
            }
            else
            {
                Debug.Log("Failed" + response.errorData.message);
            }
        });
    }

}