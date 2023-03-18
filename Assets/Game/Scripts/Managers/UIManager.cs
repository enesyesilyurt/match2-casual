using System;
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
    [SerializeField] private BottomSelectorController bottomSelectorController;
    [SerializeField] private Transform targetParent;
    [SerializeField] private TargetController targetPrefab;
    [SerializeField] private TextMeshProUGUI moveCountText;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject tryAgainPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject inGameShadow;
    [SerializeField] private GameObject continuePanel;
    [SerializeField] private Button playButton;

    private List<TargetController> targetList = new ();
    private GameObject activePanel;

    public void Setup()
    {
        playButton.onClick.AddListener(()=> GameManager.Instance.ChangeGameState(GameState.InGame));
        GameManager.Instance.GameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Home:
                SetupHomePanel();
                break;
            case GameState.InGame:
                break;
        }
    }

    private void SetupHomePanel()
    {
        bottomSelectorController.Setup();
        homePanel.SetActive(true);
        inGamePanel.SetActive(false);
        CloseActivePanel();
    }

    public void SetupInGamePanel()
    {
        homePanel.SetActive(false);
        inGamePanel.SetActive(true);
        
        inGameShadow.SetActive(false);
        failPanel.SetActive(false);
        winPanel.SetActive(false);

        targetList.Clear();
        
        var targets = LevelManager.Instance.CurrentLevel.Targets;
        for (int i = 0; i < targets.Length; i++)
        {
            var targetObject = Instantiate(targetPrefab, targetParent);
            targetList.Add(targetObject);
            targetObject.Setup(targets[i]);
        }
        
        moveCountText.text = LevelManager.Instance.CurrentLevel.maxMove.ToString();
    }

    public void OpenWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void OpenFailPanel()
    {
        tryAgainPanel.SetActive(false);
        inGameShadow.SetActive(true);
        failPanel.SetActive(true);
        continuePanel.SetActive(true);
    }

    public void OpenTryAgainPanel()
    {
        continuePanel.SetActive(false);
        tryAgainPanel.SetActive(true);
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

    #region Buttons

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        activePanel = panel;
    }

    public void ResetLevel()
    {
        LevelManager.Instance.RestartLevel();
    }

    public void GetMainMenu()
    {
        GameManager.Instance.ChangeGameState(GameState.Home);
    }

    public void CloseActivePanel()
    {
        if(activePanel == null) return;
        
        activePanel.SetActive(false);
        activePanel = null;
    }

    #endregion
}
