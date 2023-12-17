using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

public class NewsPreview : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI day;
    [SerializeField] private TextMeshProUGUI month;
    [SerializeField] private GameObject newBadge;

    public event Action<News> OnClick = delegate { };

    private News news;


    public void Initialize(News news)
    {
        this.news = news;

        title.text = news.Data.title;
        description.text = news.Data.content;
        day.text = news.Data.Date.Day.ToString();
        month.text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(news.Data.Date.Month);
        newBadge.SetActive(!news.Seen);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        newBadge.SetActive(false);
        OnClick?.Invoke(news);
    }
}
