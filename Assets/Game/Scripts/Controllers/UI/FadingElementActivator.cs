using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FadingElementActivator : ActivableUIElement
{
    private CanvasGroup canvasGroup;

    public override void Setup(bool isActive)
    {
        this.isActive = isActive;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = isActive ? 1 : 0;
        gameObject.SetActive(isActive);
    }

    public override void ChangeSituation(bool newSituation)
    {
        if(isActive == newSituation) return;
        isActive = newSituation;
        if (isActive) gameObject.SetActive(true);

        transform.localScale = Vector3.one * (isActive ? .7f : 1);
        transform.DOScale(isActive ? 1 : .7f, .3f).SetEase(isActive ? Ease.OutBack : Ease.InBack)
            .SetDelay(isActive ? inDelay : outDelay);
        
        canvasGroup.DOFade(isActive ? 1 : 0, .3f).SetDelay(isActive ? inDelay : outDelay)
            .OnComplete(()=> gameObject.SetActive(isActive));
    }
}
