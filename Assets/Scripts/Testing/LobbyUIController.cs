using Scaffold.Stateful;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController: StatefulBehaviour
{
    [SerializeField] private TextMeshProUGUI playerCount;
    [SerializeField] private Button cancelButton;

    public void SetPlayerCount(int amount, int maxAmount)
    {

    }

    public void Cancel()
    {

    }

    public class LookingForMatch: State<LobbyUIController>
    {

    }

    public class SelectMatch: State<LobbyUIController>
    {

    }
}
