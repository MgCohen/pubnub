using UnityEngine;
using Zenject;
using DG.Tweening;

public class UIElement: MonoBehaviour
{
    [Inject] protected SignalBus signalBus;

    [SerializeField] protected Canvas canvas;
    [SerializeField] protected RectTransform element;

    private bool isShowing = true;

    [Inject]
    protected virtual void Setup()
    {
        signalBus.Subscribe<ToggleUIElementSignal>(ToggleElement);
    }

    protected void ToggleElement(ToggleUIElementSignal signal)
    {
        ComponentDisplayOption option = signal.Option;
        if (option == ComponentDisplayOption.Show)
        {
            canvas.sortingOrder = signal.Screen.Layer + 1;
            if (!isShowing)
            {
                AnimateIn();
            }
        }

        else if (option == ComponentDisplayOption.Hide)
        {
            if (isShowing)
            {
                AnimateOut();
            }
        }
    }

    protected virtual void AnimateIn()
    {
        isShowing = true;
    }

    protected virtual void AnimateOut()
    {
        isShowing = false;
    }
}
