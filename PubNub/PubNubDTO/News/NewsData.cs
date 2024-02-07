using System.Collections.Generic;

public class NewsData : IGameModuleData
{
    public string Key => "News";

    public NewsData(List<News> news)
    {
        News = news;
    }

    public List<News> News = new List<News>();

}
