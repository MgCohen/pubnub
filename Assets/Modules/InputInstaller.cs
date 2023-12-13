using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{
    [SerializeField] private InputService input;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputService>().FromComponentInNewPrefab(input).UnderTransform(transform).AsSingle().NonLazy();
    }
}