using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudSave.Model;

public class PlayerData : DataCache
{
    public PlayerData(ILogger<DataCache> logger, IGameApiClient gameApiClient) : base(logger, gameApiClient)
    {
    }

    protected override async Task<Dictionary<string, string>> FetchData(IExecutionContext context)
    {
        var result = await gameApiClient.CloudSaveData.GetProtectedItemsAsync(context, context.ServiceToken, context.ProjectId, context.PlayerId);
        return result.Data.Results.ToDictionary(item => item.Key, item => item.Value.ToString());
    }

    protected override async Task SaveData(IExecutionContext context, string key, object value, bool useWriteLock)
    {
        SetItemBody item = new SetItemBody(key, value);
        if (useWriteLock)
        {
            item.WriteLock = Guid.NewGuid().ToString();
        }
        await gameApiClient.CloudSaveData.SetProtectedItemAsync(context, context.ServiceToken, context.ProjectId, context.PlayerId, item);
    }
}
