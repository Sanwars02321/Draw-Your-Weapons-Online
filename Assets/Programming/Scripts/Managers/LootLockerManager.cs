using UnityEngine;
using LootLocker.Requests;

public class LootLockerManager : MonoBehaviour
{
    public static LootLockerManager Instance;
    private bool sessionActive = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
    
    private void StartGuestSession()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                sessionActive = true;
                Debug.Log("Joined");

            }
            else
            {
                Debug.LogError("Error al iniciar sesión: " + response.errorData.message);
            }
        });
    }
    
    public void SendAllStats(PlayerStats stats)
    {
        if (!sessionActive)
        {
            Debug.LogError("No hay sesión activa en LootLocker");
            return;
        }
        
        SubmitScore("total_kills", stats.totalKills);
        SubmitScore("total_matches", stats.totalMatches);
        SubmitScore("total_wins", stats.totalWins);
        SubmitScore("best_killstreak", stats.bestKillstreak);

        // if (stats.totalDeaths > 0)
        // {
        //     int kdRatio = (stats.totalKills * 100) / stats.totalDeaths;
        //     SubmitScore("kd_ratio", kdRatio);
        // }

        // if (stats.totalMatches > 0)
        // {
        //     int winRate = (stats.totalWins * 100) / stats.totalMatches;
        //     SubmitScore("win_rate", winRate);
        // }
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
}