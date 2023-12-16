using Zenject;

public class NewsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<NewsScreenContext>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<NewsService>().AsSingle().NonLazy();
    }
}

