using System;
using UnityEngine;

[Serializable]
public class StackedScreen
{
    public StackedScreen(IScreen screen, bool destroyOnClose)
    {
        this.Screen = screen;
        this.ScreenObject = (screen as MonoBehaviour).gameObject;
        this.DestroyOnClose = destroyOnClose;
    }

    public StackedScreen(IScreen screen, IScreenContext context, bool destroyOnClose)
    {
        this.Screen = screen;
        this.ScreenObject = (screen as MonoBehaviour).gameObject;
        this.Context = context;
        this.DestroyOnClose = destroyOnClose;
    }

    public IScreen Screen { get; private set; }
    public IScreenContext Context { get; private set; }
    public bool DestroyOnClose { get; private set; }

    public GameObject ScreenObject { get; private set; }

    public void DefineContext(IScreenContext context)
    {
        Context = context;
    }
}
