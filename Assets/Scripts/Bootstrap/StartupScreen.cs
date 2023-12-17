using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartupScreen : Screen<StartupContext>
{
    [SerializeField] private Slider progress;
    [SerializeField] private TextMeshProUGUI progressText;

    protected override void Setup()
    {
        base.Setup();
        Debug.Log("Setup");
    }

    protected override void OnContextUpdated()
    {
        progress.value = Context.Progress;
        progressText.text = Context.Progress + "%";
    }
}
