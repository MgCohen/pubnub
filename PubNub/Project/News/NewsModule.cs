using Google.Protobuf.Collections;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudCode.Shared;
using Unity.Services.CloudSave.Model;

public class NewsModule : IGameModule
{
    public NewsModule(ILogger<NewsModule> logger)
    {
        this.logger = logger;
    }

    public const string Key = "News";
    
    private ILogger logger;

    public async Task<IGameModuleData> Initialize(IExecutionContext context, PlayerData playerData, GameState gameState, GameConfig gameConfig)
    {
        List<string> newsOpenned = await playerData.Get(context, Key, new List<string>());
        List<NewsContent> news = await gameConfig.Get(context, Key, new List<NewsContent>());

        List<News> newsList = news.Select(n => new News(n, newsOpenned.Contains(n.Id))).ToList();

        return new NewsData(newsList);
    }

    [CloudCodeFunction("ReadNews")]
    public async Task ReadNews(IExecutionContext context, PlayerData playerData, string newsId)
    {
        List<string> newsOpenned = await playerData.Get(context, Key, new List<string>());
        logger.LogInformation($"news was read? {newsOpenned.Contains(newsId)} ");
        if (!newsOpenned.Contains(newsId))
        {
            newsOpenned.Add(newsId);
            await playerData.Set(context, Key, newsOpenned);
        }
    }
}