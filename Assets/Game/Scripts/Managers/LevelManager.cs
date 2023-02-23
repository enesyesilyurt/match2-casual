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
    		for (var y = 0; y < CurrentLevel.Size.y; y++)
    		{
    			for (var x = 0; x < CurrentLevel.Size.x; x++)
    			{
    				var cell = boardController.Cells[x, y];

                    Colour colour = Colour.None;
                    if (CurrentLevel.Rows.Length > y && CurrentLevel.Rows[y].Colours.Length > x)
                    {
	                    colour = CurrentLevel.Rows[y].Colours[x];
                    }

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
