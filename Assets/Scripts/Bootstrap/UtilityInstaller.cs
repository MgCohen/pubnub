using UnityEngine;
using Zenject;

public class UtilityInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Coroutiner>().FromNewComponentOnNewGameObject().AsSingle();
        SignalBusInstaller.Install(Container);
    }
}