using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class NewsService : IInitializable
{
    [Inject] private IConfigService configs;
    [Inject] private ISaveService save;
    [Inject] private NewsScreenContext context;
    [Inject] private NewsCollection news;

    private List<News> currentNews = new List<News>();

    [Inject]
    private void Test()
    {
        Debug.Log(1);
    }

    public async void Initialize()
    {
        Debug.Log(news);
        configs.GetConfig("News", news);
        var newsSeen = save.GetSave<List<string>>("News", new List<string>());
        var imagesToLoad = new List<Task>();
        foreach (var newsData in news.News)
        {
            bool seen = newsSeen.Contains(newsData.id);
            News news = new News(newsData, seen);
            currentNews.Add(news);

            if(!string.IsNullOrEmpty(newsData.Thumbnail?.AssetGUID))
            {
                Task spriteLoadTask = newsData.Thumbnail.LoadAsync<Sprite>().ContinueWith(task => news.Thumbnail = task.Result);
                imagesToLoad.Add(spriteLoadTask);
            }
        }
        await Task.WhenAll(imagesToLoad);
        context.SetNews(currentNews);
    }

    public void MarkNewsAsSeen(News news)
    {
        if (news.Seen == true) return;
        news.Seen = true;
        //call cloudscript for marking and saving
    }
}


[Serializable]
public class NewsData
{
    public string id;
    public string title;
    public string content;
    public DateTime Date;
    public AssetReferenceT<Sprite> Thumbnail;
    public AssetReference View;
}

[Serializable]
public class News
{
    public News(NewsData data, bool seen)
    {
        Data = data;
        Seen = seen;
    }

    public NewsData Data;
    public bool Seen;
    public Sprite Thumbnail;
}