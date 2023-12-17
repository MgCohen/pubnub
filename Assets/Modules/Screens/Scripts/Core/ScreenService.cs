using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class ScreenService : IScreenService, IInitializable
{
    [Inject] private Coroutiner coroutiner;
    [Inject] private ScreenFactory factory;
    [Inject] private ScreenDictionary screenDictionary;
    [Inject] private ISceneServices scenes;
    [Inject] private SignalBus signals;

    public IScreen CurrentScreen { get; private set; }

    private List<StackedScreen> screenStack = new List<StackedScreen>();
    private Dictionary<Type, IScreen> sceneScreens = new Dictionary<Type, IScreen>();
    private ScreenQueue queue;
    private Transform screenHolder;

    public void Initialize()
    {
        queue = new ScreenQueue(coroutiner);
        ResetScreenStack();
        scenes.AfterSceneTransition += ResetScreenStack;
    }

    public void ResetScreenStack()
    {
        Debug.Log("Reseting screen stack");
        screenStack.Clear();
        sceneScreens.Clear();
        CurrentScreen = null;

        var screens = GameObject.FindObjectsByType<Screen>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var screen in screens)
        {
            if (screen.isActiveAndEnabled)
            {
                StackedScreen stack = new StackedScreen(screen, null, false);
                if (CurrentScreen != null)
                {
                    screen.gameObject.SetActive(false);
                    AddToStack(stack);
                }
                else
                {
                    queue.QueueSequence(OpenSequence(stack));
                }
            }
            sceneScreens.TryAdd(screen.GetType(), screen);
        }
    }

    #region Stack Operations
    private void AddToStack(StackedScreen stacked)
    {
        if (stacked != null)
        {
            screenStack.Add(stacked);
            CurrentScreen = stacked.Screen;
        }
    }

    private void RemoveFromStack(StackedScreen stacked)
    {
        if (stacked != null)
        {
            screenStack.Remove(stacked);
            if (CurrentScreen == stacked.Screen)
            {
                CurrentScreen = screenStack.LastOrDefault()?.Screen;
            }
        }
    }

    private StackedScreen GetScreen<T>() where T: Screen
    {
        return GetScreen(typeof(T));
    }

    private StackedScreen GetScreen(Type screenType)
    {
        if (TryGetSceneScreen(screenType, out IScreen screen))
        {
            return new StackedScreen(screen, false);
        }

        if (TryGetAssetScreen(screenType, out screen))
        {
            return new StackedScreen(screen, true);
        }

        return null;
    }

    private bool TryGetSceneScreen(Type screenType, out IScreen screen)
    {
        if (screenType != null && sceneScreens.TryGetValue(screenType, out var screenInstance))
        {
            screen = screenInstance;
            return true;
        }
        screen = null;
        return false;
    }

    private bool TryGetAssetScreen(Type screenType, out IScreen screen)
    {
        if (screenDictionary.TryGetScreen(screenType, out AssetReference screenAsset))
        {
            if (screenHolder == null)
            {
                screenHolder = new GameObject("Screen Holder").transform;
            }
            screen = factory.Create(screenAsset, screenHolder);
            screen.SetLayer(CurrentScreen.Layer);
            (screen as MonoBehaviour).gameObject.SetActive(false);
            return true;
        }
        screen = null;
        return false;
    }

    #endregion
    #region Navigation

    public T Open<T>(bool closeCurrent = false) where T : Screen
    {
        return Open<T>(null, closeCurrent);
    }

    public IScreen Open(Type screenType, bool closeCurrent = false)
    {
        StackedScreen stack = GetScreen(screenType);
        if (CurrentScreen == stack.Screen)
        {
            //do nothing, its the same screen with a diferent context
            //double check - this probably will be missing a notify
        }
        else if (closeCurrent && CurrentScreen != null)
        {
            Close(CurrentScreen);
        }
        else
        {
            HideCurrentScreen();
        }

        queue.QueueSequence(OpenSequence(stack));
        return stack.Screen;
    }

    public T Open<T>(ScreenContext<T> context, bool closeCurrent = false) where T : Screen
    {
        StackedScreen stack = GetScreen<T>();
        if (context != null && stack?.Screen is IScreenT screenT)
        {
            stack.DefineContext(context);
            screenT.SetContext(context);
        }

        if(CurrentScreen == stack.Screen)
        {
            //do nothing, its the same screen with a diferent context
            //double check - this probably will be missing a notify
        }
        else if (closeCurrent && CurrentScreen != null)
        {
            Close(CurrentScreen);
        }
        else
        {
            HideCurrentScreen();
        }

        queue.QueueSequence(OpenSequence(stack));
        return stack.Screen as T;
    }

    public void Close<T>() where T : Screen
    {
        StackedScreen stackedScreen = screenStack.LastOrDefault(pair => pair.Screen is T);
        if (stackedScreen != null && stackedScreen.Screen != null)
        {
            queue.QueueSequence(CloseSequence(stackedScreen));
        }
    }

    public void Close(IScreen screen)
    {
        StackedScreen stackedScreen = screenStack.LastOrDefault(stacked => stacked.Screen == screen);
        if (stackedScreen != null)
        {
            queue.QueueSequence(CloseSequence(stackedScreen));
        }
    }

    public void CloseCurrentScreen()
    {
        if (CurrentScreen != null)
        {
            Close(CurrentScreen);
        }
    }

    public void CloseAll()
    {
        for (int i = screenStack.Count - 2; i >= 0; i--)
        {
            GameObject.Destroy(screenStack[i].ScreenObject);
            screenStack.Remove(screenStack[i]);
        }
        Close(CurrentScreen);
    }

    private void FocusCurrentScreen()
    {
        if (screenStack.Count > 0)
        {
            StackedScreen stackedScreen = screenStack[^1];
            var context = stackedScreen.Context;
            var screen = stackedScreen.Screen;
            var screenObj = stackedScreen.ScreenObject;
            if (screen is IScreenT tscreen)
            {
                tscreen.SetContext(context);
            }
            screen.Focus();
        }
    }

    private void HideCurrentScreen()
    {
        if (CurrentScreen != null)
        {
            CurrentScreen.Hide();
        }
    }

    #endregion
    #region Sequences
    private IEnumerator OpenSequence(StackedScreen stacked)
    {
        var oldScreen = CurrentScreen;
        AddToStack(stacked);
        stacked.ScreenObject.SetActive(true);
        yield return stacked.Screen.Open();
        signals.Fire(new ScreenChangedSignal(oldScreen, stacked.Screen));
    }

    private IEnumerator CloseSequence(StackedScreen stacked)
    {
        RemoveFromStack(stacked);
        FocusCurrentScreen(); //to guarantee there is something "behind"
        yield return stacked.Screen.Close();
        if (stacked.DestroyOnClose)
        {
            GameObject.Destroy(stacked.ScreenObject);
        }
        else
        {
            stacked.ScreenObject.SetActive(false);
        }

        signals.Fire(new ScreenChangedSignal(stacked.Screen, CurrentScreen));
    }

    #endregion
}

public class ScreenChangedSignal
{
    public ScreenChangedSignal(IScreen from, IScreen to)
    {
        From = from;
        To = to;
    }

    public IScreen From { get; private set; }
    public IScreen To { get; private set; }
}