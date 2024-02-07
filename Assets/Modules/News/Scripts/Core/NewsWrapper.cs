using UnityEngine;

[CreateAssetMenu(menuName = "Game/News/Wrapper")]
public class NewsWrapper: ScriptableObject
{
    public NewsContent News => news;
    [SerializeField] private NewsContent news;
}