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
		    TargetManager.Instance.TargetsCompleted += LevelComplete;
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
		    CollectibleManager.Instance.GetCollectible(CollectibleType.Star).ChangeValue(CurrentLevel.StarCount);
		    CollectibleManager.Instance.GetCollectible(CollectibleType.Currency).ChangeValue(CurrentLevel.CoinCount);
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
		    FallAndFillManager.Instance.DoFalls();
		    FallAndFillManager.Instance.DoFills();
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

	    private void PrepareLevel() // TODO
	    {
		    for (var i = 0; i < boardController.Cells.Length; i++)
		    {
			     if (CurrentLevel.Blocks[i].ItemType == ItemType.None) continue;
				 var itemData = CurrentLevel.Blocks[i];
				 var cell = boardController.Cells[i];
				 Item item = (itemData.Colour == Colour.Empty || itemData.Colour == Colour.None) && itemData.ItemType == ItemType.Cube
					 ? ItemFactory.Instance.CreateRandomItem(boardController.ItemsParent)
					 : ItemFactory.Instance.CreateItem(itemData.Colour, boardController.ItemsParent, itemData.ItemType);
				 
				 cell.Item = item;
				 item.transform.position = cell.transform.position;
				 
				 if (CurrentLevel.Blocks[i].ObstacleType == ItemType.None) continue;
				 Obstacle obstacle = ItemFactory.Instance.CreateObstacle(cell, boardController.ItemsParent, itemData.ObstacleType);
    			 						
				 cell.Obstacle = obstacle;
				 obstacle.transform.position = cell.transform.position;
		    }
	    }
	}
}
