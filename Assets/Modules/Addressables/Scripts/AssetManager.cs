using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

public class AssetManager
{

    public async Task InitializeContent()
    {
        await InitializeAddressables();
        await CheckContentUpdate();
        await PreloadAssets();
    }

    private async Task InitializeAddressables()
    {
        var handle = Addressables.InitializeAsync();
        await handle.Task;
    }

    private async Task CheckContentUpdate()
    {
        var checkHandler = Addressables.CheckForCatalogUpdates(true);
        var list = await checkHandler.Task;
        Debug.Log($"found {list.Count} updates");
        if (list.Count > 0)
        {
            var updateHandler = Addressables.UpdateCatalogs(list, true);
            await updateHandler.Task;
        }
    }

    private async Task PreloadAssets()
    {
        Debug.Log("Checking preload for all needed assets");
        var sizeCheckHandler = Addressables.GetDownloadSizeAsync("Preload");
        await sizeCheckHandler.Task;
        if (sizeCheckHandler.Result > 0)
        {
            Debug.Log("Size is: " + sizeCheckHandler.Result);
            var loadHandler = Addressables.LoadResourceLocationsAsync("Preload");
            await loadHandler.Task;
            //Debug.Log(loadHandler.Result[0].PrimaryKey);
            var downloadHandler = Addressables.DownloadDependenciesAsync(new List<string>() { "Preload" }, Addressables.MergeMode.Union);
            await downloadHandler.Task;
            Addressables.Release(downloadHandler);
            Debug.Log("Download completed");
        }
        else
        {
            Debug.Log("No download needed");
        }
    }

    public async Task<IList<T>> LoadAssets<T>(params string[] labels)
    {
        var handler = Addressables.LoadAssetsAsync<Object>(labels, null);
        var objects = await handler.Task;
        if (typeof(T).IsAssignableFrom(typeof(MonoBehaviour)))
        {
            return objects.Select(o => (o as GameObject).GetComponent<T>()).ToList();
        }
        return objects.Cast<T>().ToList();
    }
}
