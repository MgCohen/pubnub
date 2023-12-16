using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public static class AssetReferenceExtensions
{
    private static Dictionary<string, AssetCounter> Counters = new Dictionary<string, AssetCounter>();
    public static async Task<T> LoadAsync<T>(this AssetReference asset) where T : Object
    {
        if (GetCounter(asset, out AssetCounter counter))
        {
            counter.Add();
            return counter.Instance as T;
        }

        T instance = await LoadAssetReference<T>(asset);
        counter.Add();
        counter.Register(instance);
        return instance;
    }

    //public static T Load<T>(this AssetReference asset) where T : Object
    //{
    //    if (GetCounter(asset, out AssetCounter counter))
    //    {
    //        counter.Add();
    //        return counter.Instance as T;
    //    }

    //    Task<T> task = LoadAssetReference<T>(asset);
    //    while (task.IsCompleted)
    //    {
    //        //lets keep waiting
    //    }
    //    T instance = task.Result;
    //    counter.Add();
    //    counter.Register(instance);
    //    return instance;
    //}

    private static bool GetCounter(AssetReference asset, out AssetCounter counter)
    {
        if (!Counters.TryGetValue(asset.RuntimeKey.ToString(), out counter))
        {
            counter = new AssetCounter(asset);
            Counters[asset.RuntimeKey.ToString()] = counter;
            return false;
        }
        return true;
    }

    private static async Task<T> LoadAssetReference<T>(AssetReference asset)
    {
        Type assetType = typeof(T);
        T assetInstance;
        if (assetType.IsAssignableFrom(typeof(MonoBehaviour)))
        {
            assetInstance = await LoadComponent<T>(asset);
        }
        else
        {
            assetInstance = await LoadAsset<T>(asset);
        }
        return assetInstance;
    }

    private static async Task<T> LoadComponent<T>(AssetReference asset)
    {
        var handle = asset.LoadAssetAsync<GameObject>();
        var gameObject = await handle.Task;
        return gameObject.GetComponent<T>();
    }

    private static async Task<T> LoadAsset<T>(AssetReference asset)
    {
        var handle = asset.LoadAssetAsync<T>();
        return await handle.Task;
    }

    public static void Unload(this AssetReference asset)
    {
        if (!GetCounter(asset, out AssetCounter counter))
        {
            return;
        }

        if (counter.Release())
        {
            asset.ReleaseAsset();
        }
    }

    public static void UnloadAll()
    {
        foreach (var counter in Counters.Values)
        {
            counter.Release();
        }
    }

    public static async Task<List<T>> LoadGroup<T>(params string[] groups) where T: class
    {
        Type assetType = typeof(T);
        var handle = Addressables.LoadAssetsAsync<Object>(groups, (a) => { });
        var items = await handle.Task;
        if (assetType.IsAssignableFrom(typeof(MonoBehaviour)))
        {
            return items.Select(item => (item as GameObject).GetComponent(assetType) as T).ToList();
        }
        else
        {
            return items.Cast<T>().ToList();
        }
    }
}

public class AssetCounter
{
    public AssetCounter(AssetReference asset)
    {
        Asset = asset;
        UseCount = 0;
    }

    public AssetReference Asset { get; private set; }
    public UnityEngine.Object Instance { get; private set; }
    public int UseCount { get; private set; }

    public void Add()
    {
        UseCount++;
    }

    public bool Release()
    {
        if (UseCount <= 0) return false;

        UseCount--;
        if (UseCount <= 0)
        {
            Asset.ReleaseAsset();
            Instance = null;
        }
        return UseCount <= 0;
    }

    public void Register(UnityEngine.Object instance)
    {
        Instance = instance;
    }
}
