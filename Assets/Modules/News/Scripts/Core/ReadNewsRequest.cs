public class ReadNewsRequest : PubnubRequest
{
    public override string Endpoint => "ReadNews";

    public ReadNewsRequest(string id)
    {
        newsId = id;
    }

    public string newsId { get; private set; }
}