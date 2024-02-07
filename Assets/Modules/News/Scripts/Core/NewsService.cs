using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class NewsService : GameModule<NewsData>
{
    [Inject] private ICloudCodeService cloudCode;
    [Inject] private NewsScreenContext context;

    public override string Key => "News";

    protected override Task Initialize(NewsData data)
    {
        context.SetNews(data.News);
        return Task.CompletedTask;
    }

    public void MarkNewsAsSeen(News news)
    {
        if (news.Seen == true) return;
        news.Seen = true;
        cloudCode.Request(new ReadNewsRequest(news.Id));
    }
}
