using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class ScreenInstaller : MonoInstaller
{
    [SerializeField] private ScreenDictionary screens;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ScreenDictionary>().FromInstance(screens);
        Container.Bind<ScreenFactory>().AsTransient();
        Container.BindInterfacesAndSelfTo<ScreenService>().AsSingle().NonLazy();


        Container.DeclareSignal<ToggleUIElementSignal>().OptionalSubscriber();
        Container.DeclareSignal<ToggleHeaderSignal>().OptionalSubscriber();
        Container.DeclareSignal<ScreenChangedSignal>().OptionalSubscriber();
    }
}
