public class PlayerStats
{
    public int totalKills = 0;
    public int totalDeaths = 0;
    public int totalWins = 0;
    public int totalRoundsWon = 0;
    public int totalMatches = 0;
    public int bestKillsSingleMatch = 0;
    public int bestKillstreak = 0;

    public int currentMatchKills = 0;
    public int currentKillstreak = 0;

    
    public void OnMatchStart()
    {
        totalMatches++;
        currentMatchKills = 0;
        currentKillstreak = 0;
    }

    public void OnDeath()
    {
        if(currentKillstreak > bestKillstreak) bestKillstreak = currentKillstreak;
        totalDeaths++;
        currentKillstreak = 0;
    }

    public void OnKill()
    {
        currentKillstreak++;
        totalKills++;
        currentMatchKills++;
        
    }

    public void OnWin()
    {
        totalWins++;
    }

    public void OnRoundWon()
    {
        totalRoundsWon++;
    }

    public void OnGameEnded()
    {
        if (currentMatchKills > bestKillsSingleMatch) 
        bestKillsSingleMatch = currentMatchKills;
    }
}