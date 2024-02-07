public class LeaderboardScore
{
    public LeaderboardScore(string playerId, string playerName, double score, int rank)
    {
        PlayerId = playerId;
        PlayerName = playerName;
        Score = score;
        Rank = rank;
    }

    public string PlayerId { get; set; }

    public string PlayerName { get; set; }

    public int Rank { get; set; }

    public double Score { get; set; }
}
