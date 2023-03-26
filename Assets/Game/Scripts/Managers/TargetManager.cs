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

    public event Action<int> MoveCountChanged;
    public event Action TargetsCompleted;
    public event Action LevelFailed;

    public void Setup()
    {
        targetCount = LevelManager.Instance.CurrentLevel.Targets.Length;
        moveCount = LevelManager.Instance.CurrentLevel.maxMove;
    }

    public void DecreaseMoveCount()
    {
        moveCount--;
        MoveCountChanged?.Invoke(moveCount);
        if (moveCount <= 0)
        {
            LevelFailed?.Invoke();
        }
    }

    public void DecreaseTarget()
    {
        targetCount--;
        if (targetCount <= 0)
        {
            TargetsCompleted?.Invoke();
        }
    }
}
