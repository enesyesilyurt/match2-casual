using System.Collections;
using System.Collections.Generic;
using Casual.Managers;
using Casual.Utilities;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private HorizontalLayoutGroup layout;
    [SerializeField] private Transform targetParent;
    [SerializeField] private TargetController targetPrefab;
    [SerializeField] private TextMeshProUGUI moveCountText;

    private List<TargetController> targetList = new ();

    public void Setup(Target[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            var targetObject = Instantiate(targetPrefab, targetParent);
            targetList.Add(targetObject);
            targetObject.Setup(targets[i]);
        }
        
        moveCountText.text = LevelManager.Instance.CurrentLevel.maxMove.ToString();
        
        
        layout.padding.left = -1600;
        layout.SetLayoutHorizontal();
    }

    public void DecreaseMoveCount(int count)
    {
        moveCountText.text = count.ToString();
    }

    public void ResetManager()
    {
        foreach (var target in targetList)
        {
            if(target != null)
                Destroy(target.gameObject);
        }
    }

    public void SetLayout(int value)
    {
        if(layout.padding.left == value) return;
        
        DOVirtual.Int(layout.padding.left, value, .3f, v =>
        {
            layout.padding.left = v;
            layout.SetLayoutHorizontal();
        }).SetEase(Ease.InSine);
    }
}
