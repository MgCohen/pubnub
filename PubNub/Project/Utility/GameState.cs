using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudSave.Model;

public class GameState : DataCache
{
    private const string GAME_STATE_KEY = "GameState";

    public GameState(ILogger<DataCache> logger, IGameApiClient gameApiClient) : base(logger, gameApiClient)
    {
    }

    protected override async Task<Dictionary<string, string>> FetchData(IExecutionContext context)
    {
        var result = await gameApiClient.CloudSaveData.GetPrivateCustomItemsAsync(context, context.ServiceToken, context.ProjectId, GAME_STATE_KEY);
        return result.Data.Results.ToDictionary(item => item.Key, item => item.Value.ToString());
    }

    protected override async Task SaveData(IExecutionContext context, string key, object value, bool useWriteLock)
    {
        SetItemBody item = new SetItemBody(key, value);
        if (useWriteLock)
        {
            item.WriteLock = Guid.NewGuid().ToString();
        }
        await gameApiClient.CloudSaveData.SetPrivateCustomItemAsync(context, context.ServiceToken, context.ProjectId, GAME_STATE_KEY, item);
    }
}
