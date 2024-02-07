using System.Collections.Generic;
using UnityEngine;

public class NewsScreenContext : ScreenContext<NewsScreen>
{
    public bool Loaded { get; private set; }
    public List<News> News { get; private set; }

    public void SetNews(List<News> news)
    {
        News = news;
        Loaded = true;
        NotifyContentUpdate();
    }
}
