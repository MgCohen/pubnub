using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Zenject;

public class UGS
{
    public async Task Initialize()
    {
        var options = new InitializationOptions().SetEnvironmentName("dev");
        await UnityServices.InitializeAsync(options);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
