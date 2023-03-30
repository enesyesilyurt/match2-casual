using Casual;
using Casual.Managers;
using Casual.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameplayUIController gameplayUIController;
    [SerializeField] private MainMenuUIController mainMenuUIController;

    private GameObject activePanel;

    public void Initialize()
    {
        gameplayUIController.Initialize();
        mainMenuUIController.Initialize();
    }

    public void ResetManager()
    {
        gameplayUIController.ResetManager();
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
