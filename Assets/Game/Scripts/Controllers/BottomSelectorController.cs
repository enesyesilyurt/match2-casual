using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BottomSelectorController : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup layout;
    [SerializeField] private RectTransform layoutBG;
    [SerializeField] private SelectionButton[] selectionButtons;

    private int currentIndex;
    
    public void Setup()
    {
        layoutBG.anchoredPosition = new Vector2(400, layoutBG.anchoredPosition.y);

        layout.padding.left = -1600;
        layout.SetLayoutHorizontal();
        
        for (int i = 0; i < selectionButtons.Length; i++)
        {
             var temp = i;
             selectionButtons[i].Text.gameObject.SetActive(false);
             selectionButtons[i].Button.onClick.AddListener(()=> SetLayout(temp));
        }
        currentIndex = 2;
        selectionButtons[currentIndex].Text.gameObject.SetActive(true);
    }

    private void SetLayout(int index)
    {
        if(layout.padding.left == index * -800) return;
        var temp = currentIndex;
        currentIndex = index;

        selectionButtons[temp].Text.gameObject.SetActive(false);
        selectionButtons[index].Text.gameObject.SetActive(true);
        
        DOVirtual.Float(220, 145, .3f,
            v => selectionButtons[temp].Rect.sizeDelta = new Vector2(v, selectionButtons[temp].Rect.sizeDelta.y));
        DOVirtual.Float(145, 220, .3f,
            v => selectionButtons[index].Rect.sizeDelta = new Vector2(v, selectionButtons[index].Rect.sizeDelta.y));
        
        DOVirtual.Float(layoutBG.anchoredPosition.x, index * 145 + 110, .3f,
            v => layoutBG.anchoredPosition = new Vector2(v, layoutBG.anchoredPosition.y));
        DOVirtual.Int(layout.padding.left, index * -800, .3f, v =>
        {
            layout.padding.left = v;
            layout.SetLayoutHorizontal();
        }).SetEase(Ease.InSine);
    }
}
