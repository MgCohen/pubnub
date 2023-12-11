using System;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;

public interface ISceneServices
{
    event Action BeforeSceneTransition; //before you start doing shit
    event Action AfterSceneTransition; //when the new scene is completely loaded and the old unloaded

    public Task LoadScene(string sceneName);

    public void RegisterTransition(SceneTransition transition);
}