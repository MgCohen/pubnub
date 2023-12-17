using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class ScreenFactory : PrefabFactory<Screen>
{
    public Screen Create(AssetReference assetReference, Transform screenHolder)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
        var prefab = handle.WaitForCompletion();

        var container = GameContext.CurrentContainer ?? Container;
        var instance = container.InstantiatePrefabForComponent<Screen>(prefab);

        instance.transform.SetParent(screenHolder);
        instance.transform.SetAsFirstSibling(); //for now, to guarantee that it is on top
        return instance;
    }

}