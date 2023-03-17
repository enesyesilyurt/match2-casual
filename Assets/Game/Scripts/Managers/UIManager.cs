using System.Collections;
using System.Collections.Generic;
using Casual.Managers;
using Casual.Utilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
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
}
