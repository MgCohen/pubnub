using System;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;

public interface ISceneServices
{
    public Task LoadScene(string sceneName);
    public Task LoadScene(SceneReference scene);

    public void RegisterTransition(SceneTransition transition);
}