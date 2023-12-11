using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Scenes/TransitionList")]
public class SceneTransitionList : ScriptableObject
{
    [SerializeField]
    private List<Transition> transitions = new List<Transition>();
    [NaughtyAttributes.Scene, SerializeField]
    private string defaultTransitionScene;

    public string GetTransition(string fromScene, string toScene)
    {
        Transition transition = transitions.FirstOrDefault(t => t.FromScene == fromScene && t.ToScene == toScene);
        return transition == null ? defaultTransitionScene : transition.TransitionScene;
    }

    [Serializable]
    private class Transition
    {
        public string FromScene => fromScene;
        [NaughtyAttributes.Scene, SerializeField]
        private string fromScene;

        public string ToScene => toScene;
        [NaughtyAttributes.Scene, SerializeField]
        private string toScene;

        public string TransitionScene => transitionScene;
        [NaughtyAttributes.Scene, SerializeField]
        private string transitionScene;
    }
}
