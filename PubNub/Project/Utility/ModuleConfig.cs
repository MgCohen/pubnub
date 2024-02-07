using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;

public class ModuleConfig : ICloudCodeSetup
{
    public void Setup(ICloudCodeConfig config)
    {
        config.Dependencies.AddSingleton(GameApiClient.Create());

        config.Dependencies.AddSingleton<PlayerData>();
        config.Dependencies.AddSingleton<GameState>();
        config.Dependencies.AddSingleton<GameConfig>();

        RegisterModule<NewsModule>(config);
        RegisterModule<AssignmentsModule>(config);
        RegisterModule<LeaderboardModule>(config);
    }

    private void RegisterModule<T>(ICloudCodeConfig config) where T: class, IGameModule
    {
        config.Dependencies.AddScoped<IGameModule, T>();
        config.Dependencies.AddScoped<T>();
    }
}