using Zenject;

public class GameModulesInstaller: MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameModulesService>().AsSingle();
    }
}