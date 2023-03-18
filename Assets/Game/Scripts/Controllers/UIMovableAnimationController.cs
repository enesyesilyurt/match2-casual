using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIMovableAnimationController : MonoBehaviour, IMovableUIElement
{
    [SerializeField] private bool isHorizontal;
    [SerializeField] private float activePoint;
    [SerializeField] private float deactivePoint;
    [SerializeField] private bool isActive;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        var x = isHorizontal ? (isActive ? activePoint : deactivePoint) : rectTransform.anchoredPosition.x;
        var y = !isHorizontal ? (isActive ? activePoint : deactivePoint) : rectTransform.anchoredPosition.y;

        rectTransform.anchoredPosition = new Vector2(x, y);
    }

    public void Activate()
    {
        if(isActive) return;
        isActive = true;
        if (isHorizontal)
        {
            DOVirtual.Float(deactivePoint, activePoint, .3f, v =>
                rectTransform.anchoredPosition = new Vector2(v, rectTransform.anchoredPosition.y)).SetEase(Ease.OutBack);
        }
        else
        {
            DOVirtual.Float(deactivePoint, activePoint, .3f, v =>
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, v)).SetEase(Ease.OutBack);
        }
    }

    public void Deactivate()
    {
        if(!isActive) return;
        isActive = false;
        if (isHorizontal)
        {
            DOVirtual.Float(activePoint, deactivePoint, .3f, v =>
                rectTransform.anchoredPosition = new Vector2(v, rectTransform.anchoredPosition.y)).SetEase(Ease.InBack);
        }
        else
        {
            DOVirtual.Float(activePoint, deactivePoint, .3f, v =>
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, v)).SetEase(Ease.InBack);
        }
    }
}
