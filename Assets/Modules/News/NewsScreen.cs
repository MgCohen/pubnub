using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class NewsScreen : InjectedScreen<NewsScreenContext>
{

    [SerializeField] private Transform newsHolder;
    [SerializeField] private GameObject spinner;

    private List<NewsPreview> previews = new List<NewsPreview>();
    private NewsPreview previewPrefab;

    protected override IEnumerator OnOpen()
    {
        ToggleState();
        return base.OnOpen();
    }

    protected override void OnContextUpdated()
    {
        base.OnContextUpdated();
        ToggleState();
    }

    private void ToggleState()
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

    }
}

public class NewsScreenContext : ScreenContext<NewsScreen>
{
    public bool Loaded { get; private set; }
    public List<News> News { get; private set; }

    public void SetNews(List<News> news)
    {
        News = news;
        Loaded = true;
        //NotifyContentUpdate();
    }
}
