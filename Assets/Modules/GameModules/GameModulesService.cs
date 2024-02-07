using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class GameModulesService
{
    [Inject] private List<IGameModule> modules;
    [Inject] private ICloudCodeService cloudCode;

    public async Task Initialize()
    {
        ModuleResponse<GameData> result = await cloudCode.Request<GameData>(new GameModulesRequest());
        await Task.WhenAll(modules.Select(m => m.Initialize(result.Response)));
    }

}
public class GameModulesRequest : PubnubRequest
{
    public override string Endpoint => "InitializeModules";
}
