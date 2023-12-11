using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(FilteredRaycaster))]
public abstract class Screen : MonoBehaviour, IScreen
{
    [Inject] protected IScreenService screens;

    protected FilteredRaycaster raycaster;
    protected Canvas canvas;
    protected RectTransform content;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (raycaster == null)
        {
            raycaster = GetComponent<FilteredRaycaster>();
        }

        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
        }

        if(content == null)
        {
            content = transform.GetChild(0) as RectTransform;
        }
    }
#endif

    public ScreenType ScreenType => screenType;
    [SerializeField] private ScreenType screenType;

    public int Layer => canvas.sortingOrder;

    public virtual IEnumerator Open()
    {
        yield return null;
    }

    public virtual IEnumerator Close()
    {
        yield return null;
    }

    public virtual void Focus() { }

    public void SetLayer(int layer)
    {
        canvas.sortingOrder = layer;
    }

}

public abstract class Screen<T> : Screen, IScreenT where T : IScreenContext
{
    protected T Context;

    public void SetContext(IScreenContext context)
    {
        if (context is not T)
        {
            throw new System.Exception("Trying to set the wrong context type on screen");
        }
        Context = (T)context;
        Context.ContextUpdated += OnContextUpdated;
    }

    protected virtual void OnContextUpdated() { }
}

public interface IScreen
{
    public ScreenType ScreenType { get; }
    public int Layer { get; }

    public IEnumerator Open();
    public void Focus();
    public IEnumerator Close();
}

public interface IScreenT : IScreen
{
    public void SetContext(IScreenContext context);
}

public enum ScreenType
{
    Window = 0,
    Popup = 1,
    Tab = 2,
}
