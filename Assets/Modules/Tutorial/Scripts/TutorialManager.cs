using UnityEngine;
using Zenject;

public class TutorialManager
{
    [Inject] private DiContainer container;

    private Tutorial activeTutorial;
    private int tutorialStep;

    public void StartTutorial(Tutorial tutorial)
    {
        if (activeTutorial != null)
        {
            return;
        }

        foreach (var step in tutorial.Steps)
        {
            container.Inject(step);
        }
        activeTutorial = tutorial;
        tutorialStep = 0;

        NextTutorialStep();
    }

    private void NextTutorialStep()
    {
        if (activeTutorial == null || tutorialStep >= activeTutorial.Steps.Count)
        {
            Debug.Log("Finishing tutorial");
            FinishTutorial();
            return;
        }

        Debug.Log(tutorialStep + " - " + activeTutorial.Steps[tutorialStep].GetType().Name);
        activeTutorial.Steps[tutorialStep++].DoStep(NextTutorialStep);
    }

    private void FinishTutorial()
    {
        //save progress
        activeTutorial = null;
        tutorialStep = 0;
    }
}
