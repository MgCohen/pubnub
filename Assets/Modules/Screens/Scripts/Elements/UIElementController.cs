using System;
using System.Collections;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;
using Zenject;

public abstract class UIElementController<T> : ScreenComponent where T : UIElement
{
    [Inject] protected SignalBus signals;
    [SerializeField] protected ComponentDisplayOption Option;

    public override void OnOpen(bool isNew)
    {
        signals.Fire(new ToggleUIElementSignal(typeof(T), Option, screen));
    }
}

public class ToggleUIElementSignal
{
    public ToggleUIElementSignal(Type elementType, ComponentDisplayOption option, IScreen screen)
    {
        ElementType = elementType;
        Option = option;
        Screen = screen;
    }

    public Type ElementType { get; protected set; }
    public ComponentDisplayOption Option { get; protected set; }
    public IScreen Screen { get; protected set; }
}

public enum ComponentDisplayOption
{
    Show = 0,
    Hide = 1,
    Ignore = 2,
}

