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
    private float buttonMaxY;
    private float buttonFirstY;
    
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
             selectionButtons[i].IconRect.localScale = Vector3.one * .7f;
        }
        currentIndex = 2;
        
        var selected = selectionButtons[currentIndex];
        buttonFirstY = selectionButtons[0].IconRect.anchoredPosition.y;
        buttonMaxY = selectionButtons[0].IconRect.anchoredPosition.y + 50;
        selected.Text.gameObject.SetActive(true);
        selected.IconRect.anchoredPosition = new Vector2(selected.IconRect.anchoredPosition.x, selected.IconRect.anchoredPosition.x + 50);
        selected.IconRect.localScale = Vector3.one;
        selected.Rect.sizeDelta = new Vector2(220, selected.Rect.sizeDelta.y);
    }

    private void SetLayout(int index)
    {
        if(currentIndex == index) return;
        
        DeselectButton(selectionButtons[currentIndex]);
        currentIndex = index;
        SelectButton(selectionButtons[index]);
        SetLayer(index);
    }

    private void SetLayer(int index)
    {
        DOVirtual.Float(layoutBG.anchoredPosition.x, index * 145 + 110, .3f,
            v => layoutBG.anchoredPosition = new Vector2(v, layoutBG.anchoredPosition.y));
        
        DOVirtual.Int(layout.padding.left, index * -800, .3f, v =>
        {
            layout.padding.left = v;
            layout.SetLayoutHorizontal();
        }).SetEase(Ease.InSine);
    }

    private void SelectButton(SelectionButton selected)
    {
        selected.Text.gameObject.transform.localScale = Vector3.one * .1f;
        selected.Text.gameObject.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBack);
        selected.Text.gameObject.SetActive(true);
        
        selected.IconRect.gameObject.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBack);

        DOVirtual.Float(selected.IconRect.anchoredPosition.x, buttonMaxY, .3f,
            v => selected.IconRect.anchoredPosition = new Vector2(selected.IconRect.anchoredPosition.x, v)).SetEase(Ease.OutBack);
        
        DOVirtual.Float(145, 220, .3f,
            v => selected.Rect.sizeDelta = new Vector2(v, selected.Rect.sizeDelta.y)).SetEase(Ease.OutBack);
    }

    private void DeselectButton(SelectionButton selected)
    {
        selected.Text.gameObject.transform.DOScale(Vector3.one * .1f, .3f).SetEase(Ease.OutBack);
        selected.Text.gameObject.SetActive(false);

        selected.IconRect.gameObject.transform.DOScale(Vector3.one * .7f, .3f).SetEase(Ease.OutBack);
        
        DOVirtual.Float(selected.IconRect.anchoredPosition.x, buttonFirstY, .3f,
            v => selected.IconRect.anchoredPosition = new Vector2(selected.IconRect.anchoredPosition.x, v)).SetEase(Ease.OutBack);
        
        DOVirtual.Float(220, 145, .3f,
            v => selected.Rect.sizeDelta = new Vector2(v, selected.Rect.sizeDelta.y)).SetEase(Ease.OutBack);
    }
}
