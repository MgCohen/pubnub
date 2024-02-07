using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.PlayerAuth.Model;

public class AssignmentsModule : IGameModule
{
    public AssignmentsModule(ILogger<AssignmentsModule> logger, LeaderboardModule leaderboard)
    {
        this.logger = logger;
        this.leaderboard = leaderboard;
    }

    public const string Key = "Assignments";

    private ILogger<AssignmentsModule> logger;
    private LeaderboardModule leaderboard;

    [CloudCodeFunction("DeliverAssignment")]
    public async Task DeliverAssignment(IExecutionContext context, IGameApiClient gameApiClient, PlayerData playerData, GameState gameState)
    {
        DailyAssignment? daily = await gameState.Get<DailyAssignment>(context, Key);
        logger.LogInformation(JsonConvert.SerializeObject(daily));
        await gameState.Set(context, Key, daily);
        AssignmentProgress? progress = await playerData.Get<AssignmentProgress>(context, Key, new AssignmentProgress());
        if(progress.timestamp == DateTime.Today)
        {
            logger.LogInformation("trying to deliver twice in the same day");
            throw new Exception("Tried to deliver assignments twice in the same day");
        }

        progress.timestamp = DateTime.Today;
        //refresh assignment data
        //score
        await playerData.Set(context, Key, progress);
    }

    [CloudCodeFunction("DailyRefresh")]
    public async Task DailyRefresh(IExecutionContext context, IGameApiClient gameApiClient, PlayerData playerData, GameState gameState)
    {
        DailyAssignment? daily = await playerData.Get<DailyAssignment>(context, Key);
        if (daily == null)
        {
            logger.LogInformation("no daily defined yet, creating new one");
            daily = new DailyAssignment();
        }
        daily.timestamp = daily.timestamp.AddDays(1);
        logger.LogInformation(daily.timestamp.ToString());
        await gameState.Set(context, Key, daily);
    }

    public async Task<IGameModuleData> Initialize(IExecutionContext context, PlayerData playerData, GameState gameState, GameConfig gameConfig)
    {
        logger.LogInformation("checking leaderboard " + (leaderboard != null));
        await Task.Yield();
        return new AssigmentData();
    }
}
