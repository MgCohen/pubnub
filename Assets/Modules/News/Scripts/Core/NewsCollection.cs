using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/News/Collection")]
public class NewsCollection : ScriptableObject
{
    [HideInInspector] public List<NewsContent> News;
    [SerializeField, JsonIgnore] private List<NewsWrapper> wrappers = new List<NewsWrapper>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            News = wrappers.Select(n => n.News).ToList();
        }
    }
#endif

}
