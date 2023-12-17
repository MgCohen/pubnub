using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/News/Collection")]
public class NewsCollection: ScriptableObject
{
    [HideInInspector] public List<NewsData> News;
    [SerializeField, JsonIgnore] private List<NewsWrapper> news = new List<NewsWrapper>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        News = news.Select(n => n.News).ToList();
    }
#endif

}
