using UnityEngine;
using System;
using TMPro;

public class PlayerScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI score;

    public void Setup(LeaderboardScore score)
    {
        throw new NotImplementedException();
    }
}
