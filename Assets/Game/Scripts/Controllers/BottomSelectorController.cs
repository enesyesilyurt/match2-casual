using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BottomSelectorController : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private HorizontalLayoutGroup layout;
    [SerializeField] private RectTransform layoutBG;
    
    private RectTransform[] buttonRects;

    private int currentIndex;
    
    public void Setup()
    {
        layoutBG.anchoredPosition = new Vector2(400, layoutBG.anchoredPosition.y);

        layout.padding.left = -1600;
        layout.SetLayoutHorizontal();

        buttonRects = new RectTransform[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
             buttonRects[i] = buttons[i].transform as RectTransform;
             var temp = i;
             buttons[i].onClick.AddListener(()=> SetLayout(temp));
        }
        currentIndex = 2;
    }

    private void SetLayout(int index)
    {
        if(layout.padding.left == index * -800) return;
        var temp = currentIndex;
        currentIndex = index;
        
        DOVirtual.Float(220, 145, .3f,
            v => buttonRects[temp].sizeDelta = new Vector2(v, buttonRects[temp].sizeDelta.y));
        
        DOVirtual.Float(145, 220, .3f,
            v => buttonRects[index].sizeDelta = new Vector2(v, buttonRects[index].sizeDelta.y));

        
        DOVirtual.Float(layoutBG.anchoredPosition.x, index * 145 + 110, .3f,
            v => layoutBG.anchoredPosition = new Vector2(v, layoutBG.anchoredPosition.y));
        
        DOVirtual.Int(layout.padding.left, index * -800, .3f, v =>
        {
            layout.padding.left = v;
            layout.SetLayoutHorizontal();
        }).SetEase(Ease.InSine);
    }
}
