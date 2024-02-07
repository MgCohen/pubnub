using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;

public class GameConfig : DataCache
{
    public GameConfig(ILogger<DataCache> logger, IGameApiClient gameApiClient) : base(logger, gameApiClient)
    {
    }

    protected override async Task<Dictionary<string, string>> FetchData(IExecutionContext context)
    {
        var result = await gameApiClient.RemoteConfigSettings.AssignSettingsGetAsync(context, context.AccessToken, context.ProjectId, context.EnvironmentId);
        return result.Data.Configs.Settings.ToDictionary(item => item.Key, item => item.Value.ToString());
    }

    protected override Task SaveData(IExecutionContext context, string key, object value, bool useWriteLock)
    {
        logger.LogInformation("Trying to save config - setting config is not implemented yet");
        throw new NotImplementedException();
    }
}
