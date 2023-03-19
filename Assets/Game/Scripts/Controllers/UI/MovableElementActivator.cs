using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MovableElementActivator : ActivableUIElement
{
    [SerializeField] private bool isHorizontal;
    [SerializeField] private float activePoint;
    [SerializeField] private float deactivePoint;
    
    private RectTransform rectTransform;

    public override void Setup(bool isActive)
    {
        rectTransform = GetComponent<RectTransform>();
        base.isActive = isActive;
        gameObject.SetActive(isActive);

        var x = isHorizontal ? (isActive ? activePoint : deactivePoint) : rectTransform.anchoredPosition.x;
        var y = !isHorizontal ? (isActive ? activePoint : deactivePoint) : rectTransform.anchoredPosition.y;

        rectTransform.anchoredPosition = new Vector2(x, y);
    }

    public override void ChangeSituation(bool newSituation)
    {
        if(isActive == newSituation) return;
        isActive = newSituation;
        if (isActive) gameObject.SetActive(true);
        
        var from = isActive ? deactivePoint : activePoint;
        var to = isActive ? activePoint : deactivePoint;

        DOVirtual.Float(from, to, .3f, v => rectTransform.anchoredPosition = new Vector2
        (
            isHorizontal ? v : rectTransform.anchoredPosition.x,
            isHorizontal ? rectTransform.anchoredPosition.y : v)
        ).SetEase(isActive ? Ease.OutBack : Ease.InBack, .8f).SetDelay(isActive ? inDelay : outDelay)
            .OnComplete(()=> gameObject.SetActive(isActive));
    }
}
