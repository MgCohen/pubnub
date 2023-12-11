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

    public async Task<IList<T>> LoadAssets<T>(string label)
    {
        var handler = Addressables.LoadAssetsAsync<Object>("label", null);
        var objects = await handler.Task;
        if (typeof(T).IsAssignableFrom(typeof(MonoBehaviour)))
        {
            return objects.Select(o => (o as GameObject).GetComponent<T>()).ToList();
        }
        return objects.Cast<T>().ToList();
    }
}
