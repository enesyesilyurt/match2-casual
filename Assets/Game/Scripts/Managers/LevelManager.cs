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
		    for (var coloumn = 0; coloumn < CurrentLevel.ColoumnCount; coloumn++)
		    {
			    for (var row = 0; row < CurrentLevel.RowCount; row++)
			    {
				    var cell = boardController.Cells[coloumn * CurrentLevel.ColoumnCount + row];
				    var colour = CurrentLevel.Blocks[CurrentLevel.RowCount * coloumn + row].Colour;
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
