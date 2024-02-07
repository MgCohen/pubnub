using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Newtonsoft.Json;

public class ScreenService : IScreenService, IInitializable
{
    [Inject] private Coroutiner coroutiner;
    [Inject] private ScreenFactory factory;
    [Inject] private ScreenDictionary screenDictionary;
    [Inject] private SignalBus signals;

    public IScreen CurrentScreen => CurrentStack?.Screen;
    private StackedScreen CurrentStack;

    private List<StackedScreen> screenStack = new List<StackedScreen>();
    private Dictionary<Type, IScreen> sceneScreens = new Dictionary<Type, IScreen>();
    private ScreenQueue queue;
    private Transform screenHolder;

    public void Initialize()
    {
        queue = new ScreenQueue(coroutiner);
        FillScreenStack();
        signals.Subscribe<SceneTransitionSignal>(ResetScreenStack);
    }

    private void ResetScreenStack(SceneTransitionSignal signal)
    {
        if (signal.State != TransitionState.AnimatingIn)
        {
            return;
        }
        FillScreenStack();
    }

    private void FillScreenStack()
    {
        Debug.Log("Reseting screen stack"); 
        screenStack.Clear();
        sceneScreens.Clear();
        CurrentStack = null;

        var screens = GameObject.FindObjectsByType<Screen>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var screen in screens)
        {
            if (screen.isActiveAndEnabled)
            {
                StackedScreen stack = new StackedScreen(screen, null, false);
                if (CurrentStack != null)
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
            CurrentStack = stacked;
        }
    }

    private void RemoveFromStack(StackedScreen stacked)
    {
        if (stacked != null)
        {
            screenStack.Remove(stacked);
            if (CurrentScreen == stacked.Screen)
            {
                CurrentStack = screenStack.LastOrDefault();
            }
        }
    }

    private StackedScreen GetScreen<T>() where T : Screen
    {
        return GetScreen(typeof(T));
    }

    private StackedScreen GetScreen(Type screenType)
    {
        if (TryGetStackedScreen(screenType, out StackedScreen stacked))
        {
            return new StackedScreen(stacked.Screen, stacked.DestroyOnClose);
        }

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

    private bool TryGetStackedScreen(Type screenType, out StackedScreen screen)
    {
        StackedScreen stacked = screenStack.Find(s => s.Screen?.GetType() == screenType);
        screen = stacked;
        return screen != null;
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
        queue.QueueSequence(OpenSequence(stack, closeCurrent));
        return stack.Screen;
    }

    public T Open<T>(ScreenContext<T> context, bool closeCurrent = false) where T : Screen
    {
        StackedScreen stack = GetScreen<T>();
        if (context != null)
        {
            stack.DefineContext(context);
        }
        queue.QueueSequence(OpenSequence(stack, closeCurrent));
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
        screenStack.Clear();
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

    private bool CheckIfScreenIsUnused(StackedScreen stacked)
    {
        return !screenStack.Any(stack => stack.Screen == stacked.Screen && stack != stacked);
    }

    #endregion
    #region Sequences
    private IEnumerator OpenSequence(StackedScreen stacked, bool closeCurrent = false, bool notify = true)
    {
        var oldScreen = CurrentScreen;

        if (CurrentScreen == stacked.Screen)
        {
            //do nothing, its the same screen with a diferent context
        }
        else if(CurrentScreen?.ScreenType is ScreenType.Overlay)
        {
            //do nothing, overlays should manage itself
        }
        else if (closeCurrent && CurrentStack != null)
        {
            yield return CloseOthersSequence(stacked, false);
        }
        else
        {
            HideCurrentScreen();
        }

        AddToStack(stacked);
        stacked.ScreenObject.SetActive(true);
        stacked.Screen.SetLayer(screenStack.Count * 2); //multiply by 2 so there is always one empty layer for extras and overlays

        if (notify)
        {
            signals.Fire(new ScreenChangedSignal(oldScreen, stacked.Screen));
        }
        yield return stacked.Screen.Open();
    }

    private IEnumerator CloseSequence(StackedScreen stacked, bool notify = true)
    {
        RemoveFromStack(stacked);
        FocusCurrentScreen(); //to guarantee there is something "behind"
        yield return stacked.Screen.Close();

        if (stacked.DestroyOnClose && CheckIfScreenIsUnused(stacked))
        {
            //only destroy if there is no other usage for this screen in the stack
            GameObject.Destroy(stacked.ScreenObject);
        }
        else
        {
            stacked.ScreenObject.SetActive(false);
        }

        if (notify)
        {
            signals.Fire(new ScreenChangedSignal(stacked.Screen, CurrentScreen));
        }
    }

    private IEnumerator CloseOthersSequence(StackedScreen stacked, bool notify = true)
    {
        var screens = screenStack.Where(st => st.Screen != CurrentScreen && st.Screen != stacked.Screen).Distinct().Select(st => st.ScreenObject);
        foreach(var screen in screens)
        {
            GameObject.Destroy(screen);
        }
        screenStack.Clear();
        yield return CloseSequence(CurrentStack, notify);
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