using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneServices : ISceneServices
{
    [Inject] private SceneTransitionList transitions;

    public event Action BeforeSceneTransition;
    public event Action AfterSceneTransition;

    private SceneInstance? currentScene;
    private SceneTransition currentTransition;

    public void RegisterTransition(SceneTransition transition)
    {
        currentTransition = transition;
    }

    public async Task LoadScene(string to)
    {
        string from = SceneManager.GetActiveScene().name;
        string transitionScene = transitions.GetTransition(from, to);
        await DoSceneTransition(transitionScene, from, to);
    }

    private async Task DoSceneTransition(string transitionScene, string fromScene, string toScene)
    {
        BeforeSceneTransition?.Invoke();
        var transition = await WaitSceneLoad(transitionScene);
        while(currentTransition == null)
        {
            //wait for the transition code to start and trigger from the scene
            await Task.Yield();
        }

        //out anim
        await currentTransition.OutAnimation();
        currentTransition.DoLoadingAnimation();
        await UnloadCurrentScene();
        currentScene = await WaitSceneLoad(toScene, () => currentTransition.TransitionDone);

        //in animation
        await currentTransition.InAnimation();
        await Addressables.UnloadSceneAsync(transition).Task;
        currentTransition = null;
        AfterSceneTransition?.Invoke();
    }

    private async Task UnloadCurrentScene()
    {
        if(currentScene == null)
        {
            var scene = SceneManager.GetActiveScene();
            var unload = SceneManager.UnloadSceneAsync(scene);
            while (!unload.isDone)
            {
                await Task.Yield();
            }
        }
        else
        {
            await Addressables.UnloadSceneAsync(currentScene.Value).Task;
        }
    }

    private async Task<SceneInstance> WaitSceneLoad(string sceneName, Func<bool> readyPredicate = null)
    {
        bool needsValidation = readyPredicate != null;
        var loadingOperation = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive, needsValidation);
        var scene = await loadingOperation.Task;
        if (needsValidation)
        {
            while (!readyPredicate.Invoke())
            {
                await Task.Yield();
            }
        }
        var activation = loadingOperation.Result.ActivateAsync();
        while (!activation.isDone)
        {
            await Task.Yield();
        }
        return scene;
    }
}
