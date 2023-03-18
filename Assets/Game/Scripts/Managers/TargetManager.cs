using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Casual.Abstracts;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using UnityEngine;

public class TargetManager : MonoSingleton<TargetManager>
{
    private int targetCount;
    private int moveCount;

    public void Setup()
    {
        targetCount = LevelManager.Instance.CurrentLevel.Targets.Length;
        moveCount = LevelManager.Instance.CurrentLevel.maxMove;
    }

    public void DecreaseMoveCount()
    {
        moveCount--;
        UIManager.Instance.DecreaseMoveCount(moveCount);
        if (moveCount <= 0)
        {
            UIManager.Instance.OpenFailPanel();
        }
    }

    public void DecreaseTarget()
    {
        targetCount--;
        if (targetCount <= 0)
        {
            LevelManager.Instance.LevelComplete();
            UIManager.Instance.OpenWinPanel();
        }
    }
}
