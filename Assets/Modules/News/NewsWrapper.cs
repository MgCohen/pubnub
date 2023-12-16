using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/News/Data")]
public class NewsWrapper: ScriptableObject
{
    public List<NewsData> data;
}