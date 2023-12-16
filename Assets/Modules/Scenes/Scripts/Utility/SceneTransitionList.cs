﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Scenes/TransitionList")]
public class SceneTransitionList : ScriptableObject
{
    [SerializeField]
    private List<Transition> transitions = new List<Transition>();

    [SerializeField] 
    private SceneReference defaultTransition;

    public string GetTransition(string fromScene, string toScene)
    {
        Transition transition = transitions.FirstOrDefault(t => t.FromScene == fromScene && t.ToScene == toScene);
        return transition == null ? defaultTransition: transition.TransitionScene;
    }

    [Serializable]
    private class Transition
    {
        public string FromScene => fromScene;
        [SerializeField]
        private SceneReference fromScene;

        public string ToScene => toScene;
        [SerializeField]
        private SceneReference toScene;
      
        public string TransitionScene => transitionScene;
        [SerializeField]
        private SceneReference transitionScene;
    }
}
