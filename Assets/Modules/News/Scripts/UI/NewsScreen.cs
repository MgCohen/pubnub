using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class NewsScreen : InjectedScreen<NewsScreenContext>
{
    [Inject] private NewsService newsService;

    [SerializeField] private Transform newsHolder;
    [SerializeField] private GameObject spinner;
    [SerializeField] private NewsPreview previewPrefab;

    private List<NewsPreview> previews = new List<NewsPreview>();

    protected override IEnumerator OnOpen()
    {
        OnContextUpdated();
        return base.OnOpen();
    }

    protected override void OnContextUpdated()
    {
        if (Context.Loaded)
        {
            ShowReadyState();
        }
        else
        {
            ShowLoadingState();
        }
    }

    private void ShowLoadingState()
    {
        spinner.SetActive(true);
        newsHolder.gameObject.SetActive(false);
    }

    private void ShowReadyState()
    {
        spinner.SetActive(false);
        newsHolder.gameObject.SetActive(true);
        ClearNews();
        FillNewsList();
    }

    private void ClearNews()
    {
        foreach (var preview in previews)
        {
            Destroy(preview.gameObject);
        }
        previews.Clear();
    }

    private void FillNewsList()
    {
        foreach (var news in Context.News)
        {
            var preview = Instantiate(previewPrefab, newsHolder);
            preview.Initialize(news);
            previews.Add(preview);
            preview.OnClick += NotifyNewsSeen;
        }
    }

    private void NotifyNewsSeen(News news)
    {
        if (!news.Seen)
        {
            newsService.MarkNewsAsSeen(news);
        }

        var thing = Addressables.InstantiateAsync(news.Content.ViewGUID, transform);
        var view = thing.WaitForCompletion().GetComponent<NewsView>();
        view.Setup(news);
    }
}
