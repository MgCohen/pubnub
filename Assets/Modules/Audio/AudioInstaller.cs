using UnityEngine;
using Zenject;

public class AudioInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AudioListener>().FromNewComponentOnNewGameObject().UnderTransform(transform).AsSingle().NonLazy();
    }
}