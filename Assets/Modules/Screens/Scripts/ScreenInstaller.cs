using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class ScreenInstaller : MonoInstaller
{
    [SerializeField] private ScreenDictionary screens;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ScreenDictionary>().FromInstance(screens);
        Container.Bind<ScreenFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<ScreenService>().AsSingle().NonLazy();
    }
}
