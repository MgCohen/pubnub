using System;

[Serializable]
public class NewsContent
{
    public string Id;
    public string Title;
    public string Content;
    public DateTime Timestamp;

    public string ViewGUID;
    public string ThumbnailGUID;
}