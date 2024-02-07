using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System.Threading.Tasks;
using System;

public class LeaderboardScreen : InjectedScreen<LeaderboardContext>
{
    [SerializeField] private PlayerScoreView scoreViewPrefab;
    [SerializeField] private Transform scrollView;
    [SerializeField] private Timer timer;
    protected override void Setup()
    {
        base.Setup();
        foreach(var score in Context.TopScores)
        {
            PlayerScoreView scoreView = Instantiate(scoreViewPrefab, scrollView);
            scoreView.Setup(score);
        }
        timer.Setup(Context.EndDate);
    }
}

public class LeaderboardContext : ScreenContext<LeaderboardScreen>
{
    public LeaderboardContext(LeaderboardScore playerScore, List<LeaderboardScore> topScores, DateTime endDate)
    {
        PlayerScore = playerScore;
        TopScores = topScores;
        EndDate = endDate;
    }

    public LeaderboardScore PlayerScore { get; private set; }
    public List<LeaderboardScore> TopScores { get; private set; }
    public DateTime EndDate { get; private set; }
}
