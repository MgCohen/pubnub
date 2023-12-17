using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenQueue
{
    public ScreenQueue(Coroutiner coroutiner)
    {
        this.coroutiner = coroutiner;
    }

    [SerializeField] private Coroutiner coroutiner;
    private Queue<IEnumerator> pendingSequences = new Queue<IEnumerator>();
    private Coroutine sequenceCO;

    public void QueueSequence(IEnumerator sequence)
    {
        pendingSequences.Enqueue(sequence);
        if (sequenceCO == null)
        {
            sequenceCO = coroutiner.StartCoroutine(Sequence());
        }
    }

    private IEnumerator Sequence()
    {
        while (pendingSequences.TryDequeue(out var next))
        {
            yield return next;
        }
        sequenceCO = null;
    }
}