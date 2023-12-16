#if UNITY_EDITOR
using UnityEditor;
#endif

using System;

[Serializable]
public class SceneReference
{
    public string SceneName;
#if UNITY_EDITOR
    public SceneAsset Scene;
#endif

    public static implicit operator string(SceneReference scene) => scene.SceneName;
}
