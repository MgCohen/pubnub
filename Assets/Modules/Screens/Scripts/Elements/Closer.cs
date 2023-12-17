using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Closer : ScreenComponent
{
    [Inject] private IScreenService screens;

    [SerializeField] private bool closeOnEsc = true;

    public override void OnOpen(bool isNew)
    {
        base.OnOpen(isNew);
        if (closeOnEsc)
        {
            enabled = true;
        }
    }

    public override void OnClose(bool isDestroying)
    {
        base.OnClose(isDestroying);
        enabled = false;
    }

    private void Update()
    {
        if (closeOnEsc && Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            Debug.Log("Trying to close");
            screens.Close(screen);
        }
    }
}
