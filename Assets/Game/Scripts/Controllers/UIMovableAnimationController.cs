using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIMovableAnimationController : MonoBehaviour
{
    [SerializeField] private bool isHorizontal;
    [SerializeField] private float activePoint;
    [SerializeField] private float deactivePoint;
    
    private bool isActive;
    private RectTransform rectTransform;

    public void Setup(bool isActive)
    {
        rectTransform = GetComponent<RectTransform>();
        this.isActive = isActive;

        var x = isHorizontal ? (isActive ? activePoint : deactivePoint) : rectTransform.anchoredPosition.x;
        var y = !isHorizontal ? (isActive ? activePoint : deactivePoint) : rectTransform.anchoredPosition.y;

        rectTransform.anchoredPosition = new Vector2(x, y);
    }

    public void ChangeSituation(bool newSituation)
    {
        if(isActive == newSituation) return;
        isActive = newSituation;
        var from = isActive ? deactivePoint : activePoint;
        var to = isActive ? activePoint : deactivePoint;

        DOVirtual.Float(from, to, .3f, v => rectTransform.anchoredPosition = new Vector2
        (
            isHorizontal ? v : rectTransform.anchoredPosition.x,
            isHorizontal ? rectTransform.anchoredPosition.y : v)
        ).SetEase(isActive ? Ease.OutBack : Ease.InBack);
    }
}
