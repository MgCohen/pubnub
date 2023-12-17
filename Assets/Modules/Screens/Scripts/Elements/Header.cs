using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Header : UIElement
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI title;

    protected override void Setup()
    {
        signalBus.Subscribe<ToggleHeaderSignal>(ToggleHeader);
    }

    private void ToggleHeader(ToggleHeaderSignal signal)
    {
        if (signal.Option == ComponentDisplayOption.Show)
        {
            if (!string.IsNullOrWhiteSpace(signal.Title))
            {
                title.text = signal.Title;
            }

            if (signal.Icon)
            {
                icon.sprite = signal.Icon;
            }
        }

        ToggleElement(signal);
    }

    protected override void AnimateIn()
    {
        base.AnimateIn();
        element.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutBack);
    }

    protected override void AnimateOut()
    {
        base.AnimateOut();
        element.DOAnchorPosY(element.rect.height * 1.2f, 0.3f).SetEase(Ease.InBack);
    }
}
