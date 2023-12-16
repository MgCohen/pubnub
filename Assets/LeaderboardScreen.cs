using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System.Threading.Tasks;

public class LeaderboardScreen : Screen
{
    private void Start()
    {

    }
}

public class LeaderboardContext: ScreenContext<LeaderboardScreen>
{
    public LeaderboardEntry PlayerScore { get; private set; }
    public List<LeaderboardEntry> TopScores { get; private set; }

    public bool Loaded { get; private set; }

    public async Task FetchScores()
    {
        var allScoresTask = LeaderboardsService.Instance.GetScoresAsync("", new GetScoresOptions() { Limit = 100 });
        var playerScoreTask = LeaderboardsService.Instance.GetPlayerScoreAsync("");
        await Task.WhenAll(allScoresTask, playerScoreTask);
        PlayerScore = playerScoreTask.Result;
        TopScores = allScoresTask.Result.Results;
        Loaded = true;
        NotifyContentUpdate();
    }
}
