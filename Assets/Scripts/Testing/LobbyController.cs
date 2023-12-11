using Colyseus;
using Colyseus.Schema;
using Server;
using System;
using System.Collections;
using UnityEngine;

public class LobbyController : ColyseusManager<LobbyController>
{
    [SerializeField] private LobbyUIController uiController;
    public async void StartLookingForMatch()
    {
        var room = await GameServer.Instance.GetRoom<MyRoomState>();
        room.OnMessage<MatchReadyMessage>("matchStart", OnMatchStarted);
        room.State.OnPlayersChange((current, previous) => OnPlayersChanged(room.State, current, previous));
        
        uiController.SetPlayerCount(room.State.players.Count, (int)room.State.config.maxPlayers);
    }

    private void OnPlayersChanged(MyRoomState state, MapSchema<Player> currentValue, MapSchema<Player> previousValue)
    {
        uiController.SetPlayerCount(currentValue.Count, (int)state.config.maxPlayers);
    }

    private void OnMatchStarted(MatchReadyMessage message)
    {
        //change scene
        //load data
    }

    private class MatchReadyMessage
    {

    }
}

