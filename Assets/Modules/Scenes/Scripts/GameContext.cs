using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class GameContext : SceneContext
{
    public static DiContainer CurrentContainer;

    protected override void RunInternal()
    {
        base.RunInternal();
        CurrentContainer = Container;

    }
}
