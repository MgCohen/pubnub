using System.Collections.Generic;
using System.Linq;
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

        List<Task> initializationSequence = services.Select(s => s.Initialize()).ToList();
        await Task.WhenAll(initializationSequence);
    }
}

public interface IUnityService
{
    public Task Initialize();
}