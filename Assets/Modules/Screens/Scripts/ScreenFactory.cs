using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class ScreenFactory : PrefabFactory<Screen>
{
    public Screen Create(AssetReference assetReference, Transform screenHolder)
    {
        var handler = assetReference.LoadAssetAsync<GameObject>();
        var prefab = handler.WaitForCompletion();
        var instance = Create(prefab);
        instance.transform.SetParent(screenHolder);
        instance.transform.SetAsFirstSibling(); //for now, to guarantee that it is on top
        assetReference.ReleaseAsset();
        return instance;
    }
}