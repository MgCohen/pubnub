using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pubnub.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudCode.Shared;
using Unity.Services.Leaderboards.Model;

public class LeaderboardModule : IGameModule
{
    public LeaderboardModule(IGameApiClient gameApiClient, ILogger<LeaderboardModule> logger)
    {
        this.gameApiClient = gameApiClient;
        this.logger = logger;
    }

    public const string LeaderboardKey = "Leaderboard";

    private IGameApiClient gameApiClient;
    private ILogger<LeaderboardModule> logger;

    public async Task<IGameModuleData> Initialize(IExecutionContext context, PlayerData playerData, GameState gameState, GameConfig gameConfig)
    {
        try
        {
            LeaderboardConfig config = await gameConfig.Get<LeaderboardConfig>(context, LeaderboardKey);
            var result = await gameApiClient.Leaderboards.GetLeaderboardScoresAsync(context, context.ServiceToken, Guid.Parse(context.ProjectId), config.LeaderboardID);
            List<LeaderboardScore> topScores = result.Data.Results.Select(e => new LeaderboardScore(e.PlayerId, e.PlayerName, e.Score, e.Rank)).ToList();
            LeaderboardScore? playerScore = topScores.FirstOrDefault(e => e.PlayerId == context.PlayerId); 
            if (playerScore == null)
            {
                try
                {
                    var playerResult = await gameApiClient.Leaderboards.GetLeaderboardPlayerScoreAsync(context, context.AccessToken, Guid.Parse(context.ProjectId), config.LeaderboardID, config.LeaderboardVersion);
                    playerScore = new LeaderboardScore(playerResult.Data.PlayerId, playerResult.Data.PlayerName, playerResult.Data.Score, playerResult.Data.Rank);
                }
                catch
                {
                    playerScore = null;
                }
            }
            var leaderboardResult = await gameApiClient.Leaderboards.GetLeaderboardVersionsAsync(context, context.AccessToken, Guid.Parse(context.ProjectId), config.LeaderboardID);
            return new LeaderboardResponse(config.LeaderboardID, playerScore, topScores, leaderboardResult.Data.NextReset);
        }
        catch (ApiException e)
        {
            logger.LogError(e, JsonConvert.SerializeObject(e.Response));
            throw new Exception("api error");
        }

    }
}
