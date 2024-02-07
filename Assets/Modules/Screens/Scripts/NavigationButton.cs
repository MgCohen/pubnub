using Scaffold.Stateful;
using System;
using System.Collections;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NavigationButton : StatefulBehaviour
{
    public override StateStrategy Strategy => StateStrategy.Variable;

    [Inject] private IScreenService screens;

    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField, Inherits(typeof(Screen))] private TypeReference screen;
    [SerializeField] private Image icon;

    public bool Selected { get; private set; }
    public TypeReference ScreenType => screen;


    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Toggle(true);
        screens.Open(screen, true);
    }

    public void Toggle(bool state)
    {
        Selected = state;
        EvaluateCurrentState();
    }

    public class ButtonState: State<NavigationButton>
    {
        [SerializeField] private bool on;
        [SerializeField] private Sprite icon;
        [SerializeField] private float size = 1;

        public override string StateName => on ? "On" : "Off";

        public override bool Evaluate()
        {
            return component.Selected == on;
        }

        public override void In()
        {
            component.icon.sprite = icon;
            component.transform.localScale = Vector3.one * size;
        }
    }
}
