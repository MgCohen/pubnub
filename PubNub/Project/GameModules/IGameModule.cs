using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;

public interface IGameModule
{
    public Task<IGameModuleData> Initialize(IExecutionContext context, PlayerData playerData, GameState gameState, GameConfig gameConfig);
}
