using UnityEngine;

[CreateAssetMenu(menuName = "Game/News/Wrapper")]
public class NewsWrapper: ScriptableObject
{
    public NewsData News => news;
    [SerializeField] private NewsData news;
}