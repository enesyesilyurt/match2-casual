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

	    public LevelConfig CurrentLevel => levels[currentLevelIndex];

	    public void Setup()
	    {
		    PrepareBoard();
		    PrepareLevel();
		    StartFalls();
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

	    private void PrepareLevel()
	    {
		    for (var column = 0; column < CurrentLevel.ColumnCount; column++)
		    {
			    for (var row = 0; row < CurrentLevel.RowCount; row++)
			    {
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
