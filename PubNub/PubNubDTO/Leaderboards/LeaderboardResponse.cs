
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

public class LeaderboardResponse : IGameModuleData
{
    public string Key => "Leaderboard";

    public LeaderboardResponse(string leaderboardName, LeaderboardScore playerScore, List<LeaderboardScore> topScores, DateTime nextReset)
    {
        LeaderboardName = leaderboardName;
        PlayerScore = playerScore;
        TopScores = topScores;
        NextReset = nextReset;
    }

    public string LeaderboardName;
    public DateTime NextReset { get; }
    public LeaderboardScore? PlayerScore { get; }

    public List<LeaderboardScore> TopScores = new List<LeaderboardScore>();
}
