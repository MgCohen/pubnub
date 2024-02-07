using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using UnityEngine;

public class SaveService : IUnityService, ISaveService
{
    private Dictionary<string, Item> playerData = new Dictionary<string, Item>();
    private Dictionary<string, Item> gameState = new Dictionary<string, Item>();

    private const string GAME_STATE_KEY = "GameState";

    public async Task Initialize()
    {
        Task<Dictionary<string, Item>> playerDataTask = CloudSaveService.Instance.Data.Player.LoadAllAsync();
        Task<Dictionary<string, Item>> gameStateTask =  CloudSaveService.Instance.Data.Custom.LoadAllAsync(GAME_STATE_KEY);
        await Task.WhenAll(playerDataTask, gameStateTask);
        playerData = playerDataTask.Result;
        gameState = playerDataTask.Result;
    }

    public T Load<T>(string key, T defaultValue = default(T))
    {
        return Load<T>(key, true, defaultValue);
    }

    public T LoadGameState<T>(string key, T defaultValue = default(T))
    {
        return Load<T>(key, false, defaultValue);
    }

    private T Load<T>(string key, bool fromPlayer, T defaultValue = default(T))
    {
        Dictionary<string, Item> data = fromPlayer ? playerData : gameState;
        if (data.TryGetValue(key, out Item value))
        {
            return value.Value.GetAs<T>();
        }
        return defaultValue;
    }

    public bool LoadLocal<T>(string key, out T defaultValue)
    {
        string json = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(json))
        {
            defaultValue = default(T);
            return false;
        }
        defaultValue = JsonConvert.DeserializeObject<T>(json);
        return true;
    }

    public void SaveLocal<T>(string key, T content)
    {
        string json = JsonConvert.SerializeObject(content);
        PlayerPrefs.SetString(key, json);
    }

    public void ClearLocal(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
}
