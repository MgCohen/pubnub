using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TestScreen2: Screen<TestScreenContext2>
{
    [SerializeField] private Image image;

    public override IEnumerator Close()
    {
        yield return image.DOColor(Color.white, 1f).WaitForCompletion();
    }

    public override IEnumerator Open()
    {
        yield return image.DOColor(Context.color, 1f).WaitForCompletion();
    }

    public override void Focus()
    {
        Debug.Log("focusing");
        image.transform.DORotate(new Vector3(0, 0, 180), 0.5f);
    }
}

public class TestScreenContext2 : ScreenContext<TestScreen2>
{
    public TestScreenContext2(Color color)
    {
        this.color = color;
    }

    public Color color;
}