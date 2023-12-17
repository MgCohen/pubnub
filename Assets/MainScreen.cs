using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MainScreen : Screen
{
    private void Update()
    {
        if (Keyboard.current[Key.A].wasPressedThisFrame)
        {
            screens.Open<NewsScreen>();
        }
    }
}
