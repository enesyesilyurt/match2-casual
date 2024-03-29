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
	    [SerializeField] private AreaManager areaManager;

	    private const string currentLevelName = "CurrentLevel";

	    public LevelConfig CurrentLevel => levels[currentLevelIndex];
	    public int CurrentLevelIndex => currentLevelIndex;

	    public event Action<Item> ItemExecuted;
	    public event Action<int> CurrentLevelChanged;
	    
	    public void Initialize()
	    {
		    currentLevelIndex = PlayerPrefs.GetInt(currentLevelName);

		    TargetManager.Instance.Initialize();
		    areaManager.Initialize();
		    
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
				    Prepare();
				    break;
		    }
	    }

	    private void CloseLevel()
	    {
		    ResetManager();
	    }

	    private void Prepare()
	    {
		    currentLevelIndex = PlayerPrefs.GetInt(currentLevelName);

		    PrepareBoard();
		    PrepareLevel();
		    StartFalls();
	    }

	    private void LevelComplete()
	    {
		    CollectibleManager.Instance.GetCollectible(CollectibleType.Star).ChangeValue(CurrentLevel.StarCount);
		    CollectibleManager.Instance.GetCollectible(CollectibleType.Currency).ChangeValue(CurrentLevel.CoinCount);
		    currentLevelIndex = currentLevelIndex + 1 >= levels.Length ? 0 : currentLevelIndex + 1;
		    CurrentLevelChanged?.Invoke(currentLevelIndex);
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
		    fallAndFillManager.Initialize(boardController);
		    FallAndFillManager.Instance.Process();
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
			     if (CurrentLevel.ItemDatas[i].ItemType == null) continue;
				 var itemData = CurrentLevel.ItemDatas[i];
				 var cell = boardController.Cells[i];

				 var item = ItemFactory.GetItem(itemData.ItemType);
				 if(item == null) continue;
				 item.ItemBase.transform.position = cell.transform.position;
				 var initializableWithData = (IInitializableWithData)item;
				 initializableWithData.InitializeWithData(itemData);
				 
				 cell.Item = item;
				 
				 // if (CurrentLevel.ItemDatas[i].ObstacleType == null) continue;
				 // Obstacle obstacle = Obstacle.SpawnObstacle(Type.GetType(itemData.ItemType), cell.transform.position, out ObstacleBase obstacleBase); //ItemFactory.Instance.CreateObstacle(cell, boardController.ItemsParent, itemData.ObstacleType);
				 // cell.Obstacle = obstacle;
		    }
	    }
	}
}
