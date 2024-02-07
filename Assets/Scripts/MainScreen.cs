using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class MainScreen : Screen
{
    [Inject] private AssignmentService assignments;
    [Inject] private TutorialManager tutorials;

    public Tutorial tutorial;

    public Button button;
    private void Start()
    {
        button.onClick.AddListener(Test);
    }

    public void Test()
    {
        tutorials.StartTutorial(tutorial);
        //assignments.Test();
    }
}
