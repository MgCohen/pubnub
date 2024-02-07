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

        title.text = news.Content.Title;
        description.text = news.Content.Content;
        day.text = news.Content.Timestamp.Day.ToString();
        month.text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(news.Content.Timestamp.Month);
        newBadge.SetActive(!news.Seen);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        newBadge.SetActive(false);
        OnClick?.Invoke(news);
    }
}
