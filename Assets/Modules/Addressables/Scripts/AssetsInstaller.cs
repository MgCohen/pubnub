using UnityEngine;
using Zenject;

public class AssetsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AssetManager>().AsSingle();
    }
}