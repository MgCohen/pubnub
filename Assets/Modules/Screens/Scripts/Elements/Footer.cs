using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Footer : UIElement
{
    protected override void AnimateIn()
    {
        base.AnimateIn();
        element.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutBack);
    }

    protected override void AnimateOut()
    {
        base.AnimateOut();
        element.DOAnchorPosY(-element.rect.height * 1.2f, 0.3f).SetEase(Ease.InBack);
    }
}
