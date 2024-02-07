using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;

public abstract class DataCache
{
    public DataCache(ILogger<DataCache> logger, IGameApiClient gameApiClient)
    {
        this.logger = logger;
        this.gameApiClient = gameApiClient;
    }

    protected ILogger<DataCache> logger;
    protected IGameApiClient gameApiClient;

    private Dictionary<string, string> cache = new Dictionary<string, string>();
    private bool initialized;

    public async Task<T> Get<T>(IExecutionContext context, string key, T defaultValue)
    {
        try
        {
            if (!initialized)
            {
                await InitializeData(context);
            }

            if (!cache.TryGetValue(key, out string? value))
            {
                logger.LogInformation($"Failed to get cached value for {key}, using default value");
                return defaultValue;
            }
            return JsonConvert.DeserializeObject<T>(value);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception($"Failed to get save data for key {key}", e);
        }
    }

    public async Task<T> Get<T>(IExecutionContext context, string key)
    {
        return await Get<T>(context, key, default);
    }

    private async Task InitializeData(IExecutionContext context)
    {
        if (!initialized)
        {
            cache = await FetchData(context);
            initialized = true;
        }
    }

    protected abstract Task<Dictionary<string, string>> FetchData(IExecutionContext context);


    public async Task Set(IExecutionContext context, string key, object value, bool useWriteLock = false)
    {
        try
        {
            await SaveData(context, key, value, useWriteLock);
            if (initialized)
            {
                cache[key] = JsonConvert.SerializeObject(value);
            }
            logger.LogInformation($"saved key {key} in {GetType().Name}");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception($"failed to set data for key {key}", e);
        }
    }

    protected abstract Task SaveData(IExecutionContext context, string key, object value, bool useWriteLock);

}