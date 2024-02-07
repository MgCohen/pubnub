using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UGSInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<UGS>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ConfigService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SaveService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CloudCodeService>().AsSingle().NonLazy();
    }
}
