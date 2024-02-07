using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;

public class GameModulesController
{
    private ILogger<GameModulesController> logger;
    private PlayerData playerData;
    private GameState gameState;
    private GameConfig gameConfig;

    public GameModulesController(ILogger<GameModulesController> logger, PlayerData playerData, GameState gameState, GameConfig gameConfig)
    {
        this.logger = logger;
        this.playerData = playerData;
        this.gameState = gameState;
        this.gameConfig = gameConfig;
    }

    [CloudCodeFunction("InitializeModules")]
    public async Task<GameData> Initialize(IExecutionContext context, IEnumerable<IGameModule> modules)
    {
        GameData gameData = new GameData();
        foreach (var module in modules)
        {
            try
            {
                IGameModuleData moduleData = await module.Initialize(context, playerData, gameState, gameConfig);
                gameData.AddModuleData(moduleData);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw new Exception($"Something wrong with module {module.GetType().Name}");
            }
        }
        return gameData;

    }
}
