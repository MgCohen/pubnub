using Colyseus;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameServer: ColyseusManager<GameServer>
{
    public ColyseusClient Client
    {
        get
        {
            if(client == null)
            {
                InitializeClient();
            }
            return client;
        }
    }
    public async Task<ColyseusRoom<T>> GetRoom<T>(Dictionary<string, object> options = null)
    {
        return await Client.JoinOrCreate<T>("my_room", options);
    }
}
