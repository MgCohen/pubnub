using Zenject;

public class AssigmentInstaller: MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AssignmentService>().AsSingle();
    }
}
