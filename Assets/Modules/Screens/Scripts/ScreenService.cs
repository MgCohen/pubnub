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

    public IScreen CurrentScreen { get; private set; }


    private List<StackedScreen> screenStack = new List<StackedScreen>();
    private Dictionary<Type, IScreen> sceneScreens = new Dictionary<Type, IScreen>();
    private ScreenQueue queue;
    private Transform screenHolder;

    public void Initialize()
    {
        queue = new ScreenQueue(coroutiner);

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

        scenes.AfterSceneTransition += CleanupScreens;
    }

    private void CleanupScreens()
    {
        Debug.Log("Cleanup");
        foreach (var stack in screenStack)
        {
            if (stack.Screen == null)
            {
                Debug.Log(1);
            }
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

    private StackedScreen GetScreen<T>() where T : Screen
    {
        if (TryGetSceneScreen<T>(out T screen))
        {
            return new StackedScreen(screen, false);
        }

        if (TryGetAssetScreen<T>(out screen))
        {
            return new StackedScreen(screen, true);
        }

        return null;
    }

    private bool TryGetSceneScreen<T>(out T screen) where T : Screen
    {
        if (sceneScreens.TryGetValue(typeof(T), out var screenInstance))
        {
            screen = screenInstance as T;
            return true;
        }
        screen = null;
        return false;
    }

    private bool TryGetAssetScreen<T>(out T screen) where T : Screen
    {
        if (screenDictionary.TryGetScreen<T>(out AssetReference screenAsset))
        {
            if (screenHolder == null)
            {
                screenHolder = new GameObject("Screen Holder").transform;
            }
            screen = factory.Create(screenAsset, screenHolder) as T;
            screen.SetLayer(CurrentScreen.Layer);
            Debug.Log(1);
            screen.gameObject.SetActive(false);
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

    public T Open<T>(ScreenContext<T> context, bool closeCurrent = false) where T : Screen
    {
        StackedScreen stack = GetScreen<T>();
        if (context != null && stack.Screen is IScreenT screenT)
        {
            stack.DefineContext(context);
            screenT.SetContext(context);
        }

        if(CurrentScreen == stack.Screen)
        {
            //do nothing, its the same screen with a diferent context
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

            screenObj.SetActive(true);
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
            StackedScreen stackedScreen = screenStack[^1];
            var screenObj = stackedScreen.ScreenObject;
            Debug.Log(4);
            screenObj.gameObject.SetActive(false);
        }
    }

    #endregion
    #region Sequences
    private IEnumerator OpenSequence(StackedScreen stacked)
    {
        AddToStack(stacked);
        stacked.ScreenObject.SetActive(true);
        yield return stacked.Screen.Open();
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
            Debug.Log(3);
            stacked.ScreenObject.SetActive(false);
        }
    }

    #endregion
}
