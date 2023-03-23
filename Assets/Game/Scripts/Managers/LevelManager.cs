using System;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual.Managers
{
	public class LevelManager : MonoSingleton<LevelManager>
	{
	    [SerializeField] private int currentLevelIndex;
	    [SerializeField] private LevelConfig[] levels;
	    [SerializeField] private BoardController boardController;
	    [SerializeField] private FallAndFillManager fallAndFillManager;

	    private const string currentLevelName = "CurrentLevel";

	    public LevelConfig CurrentLevel => levels[currentLevelIndex];

	    public event Action<Item> ItemExecuted;
	    
	    public void Setup()
	    {
		    GameManager.Instance.GameStateChanged += OnGameStateChanged;
	    }

	    private void OnGameStateChanged(GameState newState)
	    {
		    switch (newState)
		    {
			    case GameState.Home:
				    CloseLevel();
				    break;
			    case GameState.InGame:
				    SetupLevel();
				    break;
		    }
	    }

	    private void CloseLevel()
	    {
		    ResetManager();
	    }

	    private void SetupLevel()
	    {
		    currentLevelIndex = PlayerPrefs.GetInt(currentLevelName);
		    ItemFactory.Instance.Setup();
		    PrepareBoard();
		    PrepareLevel();
		    TargetManager.Instance.Setup();
		    UIManager.Instance.SetupInGamePanel();
		    StartFalls();
	    }

	    public void LevelComplete()
	    {
		    currentLevelIndex = currentLevelIndex + 1 >= levels.Length ? 0 : currentLevelIndex + 1;
		    PlayerPrefs.SetInt(currentLevelName, currentLevelIndex);
	    }

	    public void RestartLevel()
	    {
		    ResetManager();
		    GameManager.Instance.ChangeGameState(GameState.InGame);
	    }

	    private void PrepareBoard()
	    {
		    boardController.Prepare();
	    }

	    private void StartFalls()
	    {
		    fallAndFillManager.Init(boardController);
		    fallAndFillManager.StartFalls();
	    }

	    private void ResetManager()
	    {
		    UIManager.Instance.ResetManager();
		    boardController.ResetBoard();
	    }

	    public void ItemExecute(Item item)
	    {
		    ItemExecuted?.Invoke(item);
	    }

	    private void PrepareLevel()
	    {
		    for (var x = 0; x < CurrentLevel.GridWidth; x++)
		    {
			    for (var y = 0; y < CurrentLevel.GridHeight; y++)
			    {
				    if (CurrentLevel.Blocks[y * CurrentLevel.GridWidth + x].ItemType == ItemType.None) continue;
				    var cell = boardController.Cells[y * CurrentLevel.GridWidth + x];
				    var colour = CurrentLevel.Blocks[y * CurrentLevel.GridWidth + x].Colour;
				    var item = colour == Colour.None
					    ? ItemFactory.Instance.CreateRandomItem(boardController.ItemsParent)
					    : ItemFactory.Instance.CreateItem(colour, boardController.ItemsParent);
                    
				    if (item == null) continue;
    				 						
				    cell.Item = item;
				    item.transform.position = cell.transform.position;
			    }
		    }
	    }
	}
}
