﻿using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Zenject;

public class ConfigService : IConfigService, IUnityService
{
    public struct userAttributes { }
    public struct appAttributes { }

    private JObject config;

    public async Task Initialize()
    {
        RuntimeConfig runTimeConfig = await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
        config = runTimeConfig.config;
    }

    public T GetConfig<T>(string key)
    {
        if (config.TryGetValue(key, out var token))
        {
            return token.ToObject<T>();
        }

        return default(T);
    }
}
