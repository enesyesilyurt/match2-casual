using Casual;
using Casual.Managers;
using Casual.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private BottomSelectorController bottomSelectorController;
    [SerializeField] private InGameUIController inGameUIController;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject homePanel;
    [SerializeField] private Button playButton;

    private GameObject activePanel;

    public void Initialize()
    {
        inGameUIController.Initialize();
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
        inGameUIController.SetupPanel();
    }

    public void ResetManager()
    {
        inGameUIController.ResetManager();
    }

    #region Buttons

    public void OpenPanel(GameObject panel)
    {
        CloseActivePanel();
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
