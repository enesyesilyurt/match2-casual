using System;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using NaughtyAttributes;
using UnityEngine;

namespace Casual.Managers
{
	public class LevelManager : MonoSingleton<LevelManager>
	{
	    [SerializeField] private int currentLevelIndex;
	    [SerializeField] private LevelConfig[] levels;
	    [SerializeField] private BoardController boardController;
	    [SerializeField] private FallAndFillManager fallAndFillManager;

	    public LevelConfig CurrentLevel => levels[currentLevelIndex];

	    public event Action<Item> ItemExecuted; 

	    public void Setup()
	    {
		    PrepareBoard();
		    PrepareLevel();
		    TargetManager.Instance.Setup();
		    UIManager.Instance.Setup(CurrentLevel.Targets);
		    StartFalls();
	    }

	    [Button]
	    public void GetNextLevel()
	    {
		    currentLevelIndex = currentLevelIndex + 1 >= levels.Length ? 0 : currentLevelIndex + 1;
		    ResetManager();
		    Setup();
	    }

	    public void RestartLevel()
	    {
		    ResetManager();
		    Setup();
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
	    
	    public void ResetManager()
	    {
		    UIManager.Instance.ResetManager();
		    fallAndFillManager.StopFalls();
		    boardController.ResetBoard();
	    }

	    public void ItemExecute(Item item)
	    {
		    ItemExecuted?.Invoke(item);
	    }

	    private void PrepareLevel()
	    {
		    for (var column = 0; column < CurrentLevel.ColumnCount; column++)
		    {
			    for (var row = 0; row < CurrentLevel.RowCount; row++)
			    {
				    if (CurrentLevel.Blocks[column * CurrentLevel.ColumnCount + row].ItemType == ItemType.None) continue;
				    var cell = boardController.Cells[column * CurrentLevel.ColumnCount + row];
				    var colour = CurrentLevel.Blocks[CurrentLevel.RowCount * column + row].Colour;
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
