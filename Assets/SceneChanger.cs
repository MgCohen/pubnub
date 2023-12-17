using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneChanger : MonoBehaviour
{
    [Inject] private ISceneServices scenes;

    public string nextScene;
    [NaughtyAttributes.Button]
    public void Change()
    {
        scenes.LoadScene(nextScene);
    }
}
