using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TutorialInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TutorialManager>().AsSingle();
    }
}
