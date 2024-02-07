public class News
{
    public News(NewsContent content, bool seen)
    {
        Content = content;
        Seen = seen;
    }

    public string Id => Content.Id;

    public NewsContent Content;
    public bool Seen;
}
