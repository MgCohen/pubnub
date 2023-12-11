using Colyseus;
using Colyseus.Schema;
using Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : ColyseusManager<Client>
{
    ColyseusRoom<MyRoomState> room;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("initializing");
            InitializeClient();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Creating and joining room");
            CreateOrJoinRoom();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            room.Send("RegisterMove", new TurnObject(room.State.turns.Count, new TurnAction(1, 1), new TurnAction(2, 2)));
        }
    }

    private async void CreateOrJoinRoom()
    {
        Dictionary<string, object> options = new Dictionary<string, object>()
        {
            {"auth", SystemInfo.deviceUniqueIdentifier }
        };
        room = await client.JoinOrCreate<MyRoomState>("my_room", options);
        room.State.turns.OnChange((a, b) => { Debug.Log("on turn changed"); });
        Debug.Log("created room");

        room.OnMessage<TurnPreview>("previewMove", (p) => { Debug.Log("received turn preview"); });
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        if (room != null)
        {
            room.Leave(true);
        }
    }
}

public class TurnPreview
{
    public string playerID;
    public TurnObject playerTurn;
}

public class TurnObject
{
    public TurnObject()
    {

    }

    public TurnObject(int turn, params TurnAction[] actions)
    {
        this.turnNumber = turn;
        this.playerTurn = actions;
    }

    public int turnNumber = 0;
    public TurnAction[] playerTurn;
}

public class TurnAction
{
    public TurnAction()
    {

    }

    public TurnAction(int action, int target)
    {
        this.actionID = action;
        this.target = target;
    }

    public int actionID = 0;
    public int target = 0;
}
