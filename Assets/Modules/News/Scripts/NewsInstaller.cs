using UnityEngine;
using Zenject;

public class NewsInstaller : MonoInstaller
{
    [SerializeField] private NewsCollection collection;

    public override void InstallBindings()
    {
        Container.Bind<NewsScreenContext>().FromNew().AsSingle();
        Container.Bind<NewsCollection>().FromInstance(collection);
        Container.BindInterfacesAndSelfTo<NewsService>().AsSingle().NonLazy();
    }
}

