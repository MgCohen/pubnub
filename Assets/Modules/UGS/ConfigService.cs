using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using UnityEngine;
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

    public void GetConfig<T>(string key, T thing) where T : ScriptableObject
    {
        if(config.TryGetValue(key, out var token))
        {
            Debug.Log(token.ToString());
            JsonUtility.FromJsonOverwrite(token.ToString(), thing);
        }
    }
}
