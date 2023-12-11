using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestScreen: Screen
{
    [SerializeField] private Image image;

    public override IEnumerator Close()
    {
        yield return image.transform.DOScale(0.3f, 1f).SetEase(Ease.OutBack).WaitForCompletion();
    }

    public override IEnumerator Open()
    {
        yield return image.transform.DOScale(1f, 1f).SetEase(Ease.InBack).From(0.3f).WaitForCompletion();
    }

    public override void Focus()
    {
        Debug.Log("focusing");
        image.transform.DORotate(new Vector3(0, 0, 180), 0.5f);
    }
}
