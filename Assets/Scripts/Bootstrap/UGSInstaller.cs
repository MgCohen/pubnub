using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UGSInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<UGS>().AsSingle();
    }
}
