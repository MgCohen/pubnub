using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TypeReferences;
using UnityEngine;
using Zenject;

public class Tutorial : ScriptableObject
{
    public string Key => key;
    [SerializeField] private string key;

    public DynamicList<TutorialStep> Steps => steps;
    [SerializeField] private DynamicList<TutorialStep> steps = new DynamicList<TutorialStep>();
}

[Serializable]
public abstract class TutorialStep
{
    public abstract void DoStep(Action onComplete);
}

public class DialogueStep : TutorialStep
{
    [Inject] private IDialogueService dialogues;
    [SerializeField] private Dialogue dialogue;

    public override void DoStep(Action onComplete)
    {
        dialogues.ShowDialogue(dialogue, onComplete);
    }
}

public class WaitStep : TutorialStep
{
    [Inject] private Coroutiner coroutiner;
    [Inject] private InputService input;

    [SerializeField] private float delay;

    public override void DoStep(Action onComplete)
    {
        coroutiner.StartCoroutine(WaitCO(onComplete));
    }

    private IEnumerator WaitCO(Action onComplete)
    {
        input.ToggleInput(false);
        yield return new WaitForSeconds(delay);
        input.ToggleInput(true);
        onComplete?.Invoke();
    }
}

public class ClickStep : TutorialStep
{
    [Inject] private TagManager tags;
    [Inject] private InputService input;
    [Inject] private SignalBus signals;
    [Inject] private IHighlightService highlight;

    [SerializeField] private GameTag tag;
    [SerializeField] private bool highlightTarget;
    [SerializeField] private float rotation = 45f;

    public override void DoStep(Action onComplete)
    {
        if (highlightTarget)
        {
            Highlight.PointerOptions options = new Highlight.PointerOptions(rotation);
            var targets = tags.GetAllTaggedObjects(tag);
            foreach(var target in targets)
            {
                highlight.Highlight(target.gameObject, options);
            }
        }

        input.FilterForTag(tag);
        signals.Subscribe<ScreenClickedSignal>(OnScreenClicked);

        void OnScreenClicked(ScreenClickedSignal signal)
        {
            if (signal.Results.Count > 0 && signal.TopResult.gameObject.TryGetComponent<Tagger>(out Tagger tagger) && tagger.ContainsTag(tag))
            {
                signals.Unsubscribe<ScreenClickedSignal>(OnScreenClicked);
                onComplete?.Invoke();
                input.ClearFilters();

                if (highlightTarget)
                {
                    var targets = tags.GetAllTaggedObjects(tag);
                    foreach (var target in targets)
                    {
                        highlight.RemoveHighlight(target.gameObject);
                    }
                }
            }
        }
    }
}

public class CloseDialogueStep : TutorialStep
{
    [Inject] private IDialogueService dialogue;

    public override void DoStep(Action onComplete)
    {
        dialogue.CloseDialogue();
        onComplete?.Invoke();
    }
}

public class WaitScreenStep : TutorialStep
{
    [Inject] private SignalBus signals;

    [SerializeField, Inherits(typeof(IScreen))] private TypeReference screenType;
    [SerializeField] private ScreenOperation operation;

    public override void DoStep(Action onComplete)
    {
        signals.Subscribe<ScreenChangedSignal>(OnScreenChanged);

        void OnScreenChanged(ScreenChangedSignal signal)
        {
            IScreen screen = operation is ScreenOperation.Open ? signal.To : signal.From;
            if (screen.GetType() == screenType.Type)
            {
                signals.Unsubscribe<ScreenChangedSignal>(OnScreenChanged);
                onComplete?.Invoke();
            }
        }
    }

    private enum ScreenOperation
    {
        Open,
        Close
    }
}
