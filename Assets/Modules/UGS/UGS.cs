using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using Zenject;

public class UGS
{
    [Inject] private List<IUnityService> services = new List<IUnityService>();

    public async Task Initialize()
    {
        var options = new InitializationOptions().SetEnvironmentName("dev");
        await UnityServices.InitializeAsync(options);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        List<Task> initializationSequence = new List<Task>();
        foreach (var service in services)
        {
            initializationSequence.Add(service.Initialize());
        }
        await Task.WhenAll(initializationSequence);
    }
}

public interface IUnityService
{
    public Task Initialize();
}