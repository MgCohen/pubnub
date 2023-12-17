using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class NavigationBar : MonoBehaviour
{
    [Inject] private IScreenService screens;
    [Inject] private SignalBus signals;

    [SerializeField] private List<NavigationButton> buttons = new List<NavigationButton>();
    private NavigationButton activeButton;
    private List<Type> watchedScreenTypes = new List<Type>();

    [Inject]
    private void Setup()
    {
        signals.Subscribe<ScreenChangedSignal>(CheckButtons);
        watchedScreenTypes = buttons.Select(b => b.ScreenType.Type).ToList();
    }

    private void CheckButtons(ScreenChangedSignal signal)
    {
        Type screenType = signal.To.GetType();
        if(watchedScreenTypes.Contains(screenType) && activeButton != null && activeButton.ScreenType.Type != screenType)
        {
            ToggleButton(activeButton, false);
        }

        foreach(var button in buttons)
        {
            if(button.ScreenType.Type == screenType)
            {
                ToggleButton(button, true);
                return;
            }
        }
    }

    private void ToggleButton(NavigationButton button, bool state)
    {
        if (state)
        {
            activeButton = button;
        }
        button.Toggle(state);
    }
}
