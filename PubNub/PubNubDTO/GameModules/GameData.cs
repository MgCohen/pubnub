using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public class GameData
{
    private Dictionary<string, IGameModuleData> modulesData = new Dictionary<string, IGameModuleData>();
    public string RawResponse;

    public void AddModuleData(IGameModuleData data)
    {
        modulesData[data.Key] = data;
    }

    public T GetModuleData<T>() where T: IGameModuleData
    {
        T dummyT = (T)FormatterServices.GetUninitializedObject(typeof(T));
        return (T)modulesData.GetValueOrDefault(dummyT.Key);
    }

    [OnSerializing]
    private void OnSerialize(StreamingContext context)
    {
        if (string.IsNullOrEmpty(RawResponse))
        {
            RawResponse = JsonConvert.SerializeObject(modulesData, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
        }
    }

    [OnDeserialized]
    private void OnDeserialize(StreamingContext context)
    {
        if (modulesData.Count <= 0 && !string.IsNullOrEmpty(RawResponse))
        {
            modulesData = JsonConvert.DeserializeObject<Dictionary<string, IGameModuleData>>(RawResponse, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
        }
    }
}
