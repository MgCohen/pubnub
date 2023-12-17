using System;
using System.Collections;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NavigationToggle : MonoBehaviour
{
    [Inject] private IScreenService screens;

    [SerializeField] private Button button;
    [SerializeField, Inherits(typeof(Screen))] private TypeReference screen;


    private void Awake()
    {
        Debug.Log(1);
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Debug.Log(2);
        screens.Open(screen);
    }
}
