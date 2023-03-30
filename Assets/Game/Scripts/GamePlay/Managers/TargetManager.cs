using System;
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

    public void Initialize()
    {
        GameManager.Instance.GameStateChanged += OnGameStateChanged;
    }

    private void Prepare()
    {
        targetCount = LevelManager.Instance.CurrentLevel.Targets.Count;
        moveCount = LevelManager.Instance.CurrentLevel.MaxMove;
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
    
    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Home:
                break;
            case GameState.InGame:
                Prepare();
                break;
        }
    }
}
