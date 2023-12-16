using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave.Models;
using UnityEngine;

public class SaveService : IUnityService, ISaveService
{
    private Dictionary<string, Item> data = new Dictionary<string, Item>();

    public async Task Initialize()
    {
        data = await Unity.Services.CloudSave.CloudSaveService.Instance.Data.Player.LoadAllAsync();
    }

    public T GetSave<T>(string key, T defaultValue = default(T))
    {
        if (data.TryGetValue(key, out Item value))
        {
            return value.Value.GetAs<T>();
        }
        return defaultValue;
    }
}
