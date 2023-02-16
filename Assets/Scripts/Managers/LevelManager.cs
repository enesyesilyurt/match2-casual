using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private LevelScriptable[] levelDatas;
    [SerializeField] private BoardController boardController;
	[SerializeField] private FallAndFillManager fallAndFillManager;

	private ItemType[] currentLevelsItems = new ItemType[81];
	private LevelData levelData;

	private void Start()
	{
		PrepareBoard();
		PrepareLevel();
		StartFalls();
	}

	private void PrepareBoard()
	{
		boardController.Prepare();
	}
	
	private void DestroyBoard()
	{
		Destroy(boardController);
	}

	private void StartFalls()
	{
		fallAndFillManager.Init(boardController, levelData);
		fallAndFillManager.StartFalls();
	}
	
	private void PrepareLevel()
	{
		AttachLevelReferences(0);

		for (var y = 0; y < 9; y++)
		{
			for (var x = 0; x < 9; x++)
			{
				var cell = boardController.Cells[x, y];

				var itemType = currentLevelsItems[(8-y) * 9 + (x)];
				var item = ItemFactory.Instance.CreateItem(itemType, boardController.ItemsParent);
				if (item == null) continue;
					 					
				cell.Item = item;
				item.transform.position = cell.transform.position;
			}
		}
	}

	private void AttachLevelReferences(int levelIndex)
	{
		currentLevelsItems[0] = levelDatas[levelIndex].Row1.Coloum1;
		currentLevelsItems[1] = levelDatas[levelIndex].Row1.Coloum2;
		currentLevelsItems[2] = levelDatas[levelIndex].Row1.Coloum3;
		currentLevelsItems[3] = levelDatas[levelIndex].Row1.Coloum4;
		currentLevelsItems[4] = levelDatas[levelIndex].Row1.Coloum5;
		currentLevelsItems[5] = levelDatas[levelIndex].Row1.Coloum6;
		currentLevelsItems[6] = levelDatas[levelIndex].Row1.Coloum7;
		currentLevelsItems[7] = levelDatas[levelIndex].Row1.Coloum8;
		currentLevelsItems[8] = levelDatas[levelIndex].Row1.Coloum9;
		
		currentLevelsItems[9]  = levelDatas[levelIndex].Row2.Coloum1;
		currentLevelsItems[10] = levelDatas[levelIndex].Row2.Coloum2;
		currentLevelsItems[11] = levelDatas[levelIndex].Row2.Coloum3;
		currentLevelsItems[12] = levelDatas[levelIndex].Row2.Coloum4;
		currentLevelsItems[13] = levelDatas[levelIndex].Row2.Coloum5;
		currentLevelsItems[14] = levelDatas[levelIndex].Row2.Coloum6;
		currentLevelsItems[15] = levelDatas[levelIndex].Row2.Coloum7;
		currentLevelsItems[16] = levelDatas[levelIndex].Row2.Coloum8;
		currentLevelsItems[17] = levelDatas[levelIndex].Row2.Coloum9;
		
		currentLevelsItems[18] = levelDatas[levelIndex].Row3.Coloum1;
		currentLevelsItems[19] = levelDatas[levelIndex].Row3.Coloum2;
		currentLevelsItems[20] = levelDatas[levelIndex].Row3.Coloum3;
		currentLevelsItems[21] = levelDatas[levelIndex].Row3.Coloum4;
		currentLevelsItems[22] = levelDatas[levelIndex].Row3.Coloum5;
		currentLevelsItems[23] = levelDatas[levelIndex].Row3.Coloum6;
		currentLevelsItems[24] = levelDatas[levelIndex].Row3.Coloum7;
		currentLevelsItems[25] = levelDatas[levelIndex].Row3.Coloum8;
		currentLevelsItems[26] = levelDatas[levelIndex].Row3.Coloum9;
		
		currentLevelsItems[27] = levelDatas[levelIndex].Row4.Coloum1;
		currentLevelsItems[28] = levelDatas[levelIndex].Row4.Coloum2;
		currentLevelsItems[29] = levelDatas[levelIndex].Row4.Coloum3;
		currentLevelsItems[30] = levelDatas[levelIndex].Row4.Coloum4;
		currentLevelsItems[31] = levelDatas[levelIndex].Row4.Coloum5;
		currentLevelsItems[32] = levelDatas[levelIndex].Row4.Coloum6;
		currentLevelsItems[33] = levelDatas[levelIndex].Row4.Coloum7;
		currentLevelsItems[34] = levelDatas[levelIndex].Row4.Coloum8;
		currentLevelsItems[35] = levelDatas[levelIndex].Row4.Coloum9;
		
		currentLevelsItems[36] = levelDatas[levelIndex].Row5.Coloum1;
		currentLevelsItems[37] = levelDatas[levelIndex].Row5.Coloum2;
		currentLevelsItems[38] = levelDatas[levelIndex].Row5.Coloum3;
		currentLevelsItems[39] = levelDatas[levelIndex].Row5.Coloum4;
		currentLevelsItems[40] = levelDatas[levelIndex].Row5.Coloum5;
		currentLevelsItems[41] = levelDatas[levelIndex].Row5.Coloum6;
		currentLevelsItems[42] = levelDatas[levelIndex].Row5.Coloum7;
		currentLevelsItems[43] = levelDatas[levelIndex].Row5.Coloum8;
		currentLevelsItems[44] = levelDatas[levelIndex].Row5.Coloum9;
		
		currentLevelsItems[45] = levelDatas[levelIndex].Row6.Coloum1;
		currentLevelsItems[46] = levelDatas[levelIndex].Row6.Coloum2;
		currentLevelsItems[47] = levelDatas[levelIndex].Row6.Coloum3;
		currentLevelsItems[48] = levelDatas[levelIndex].Row6.Coloum4;
		currentLevelsItems[49] = levelDatas[levelIndex].Row6.Coloum5;
		currentLevelsItems[50] = levelDatas[levelIndex].Row6.Coloum6;
		currentLevelsItems[51] = levelDatas[levelIndex].Row6.Coloum7;
		currentLevelsItems[52] = levelDatas[levelIndex].Row6.Coloum8;
		currentLevelsItems[53] = levelDatas[levelIndex].Row6.Coloum9;
		
		currentLevelsItems[54] = levelDatas[levelIndex].Row7.Coloum1;
		currentLevelsItems[55] = levelDatas[levelIndex].Row7.Coloum2;
		currentLevelsItems[56] = levelDatas[levelIndex].Row7.Coloum3;
		currentLevelsItems[57] = levelDatas[levelIndex].Row7.Coloum4;
		currentLevelsItems[58] = levelDatas[levelIndex].Row7.Coloum5;
		currentLevelsItems[59] = levelDatas[levelIndex].Row7.Coloum6;
		currentLevelsItems[60] = levelDatas[levelIndex].Row7.Coloum7;
		currentLevelsItems[61] = levelDatas[levelIndex].Row7.Coloum8;
		currentLevelsItems[62] = levelDatas[levelIndex].Row7.Coloum9;
		
		currentLevelsItems[63] = levelDatas[levelIndex].Row8.Coloum1;
		currentLevelsItems[64] = levelDatas[levelIndex].Row8.Coloum2;
		currentLevelsItems[65] = levelDatas[levelIndex].Row8.Coloum3;
		currentLevelsItems[66] = levelDatas[levelIndex].Row8.Coloum4;
		currentLevelsItems[67] = levelDatas[levelIndex].Row8.Coloum5;
		currentLevelsItems[68] = levelDatas[levelIndex].Row8.Coloum6;
		currentLevelsItems[69] = levelDatas[levelIndex].Row8.Coloum7;
		currentLevelsItems[70] = levelDatas[levelIndex].Row8.Coloum8;
		currentLevelsItems[71] = levelDatas[levelIndex].Row8.Coloum9;
		
		currentLevelsItems[72] = levelDatas[levelIndex].Row9.Coloum1;
		currentLevelsItems[73] = levelDatas[levelIndex].Row9.Coloum2;
		currentLevelsItems[74] = levelDatas[levelIndex].Row9.Coloum3;
		currentLevelsItems[75] = levelDatas[levelIndex].Row9.Coloum4;
		currentLevelsItems[76] = levelDatas[levelIndex].Row9.Coloum5;
		currentLevelsItems[77] = levelDatas[levelIndex].Row9.Coloum6;
		currentLevelsItems[78] = levelDatas[levelIndex].Row9.Coloum7;
		currentLevelsItems[79] = levelDatas[levelIndex].Row9.Coloum8;
		currentLevelsItems[80] = levelDatas[levelIndex].Row9.Coloum9;
	}

	// private void PrepareLevel()
	// {
	// 	_levelData = LevelDataFactory.CreateLevelData(CurrentLevel);
	//
	// 	for (var y = 0; y < _levelData.GridData.GetLength(0); y++)
	// 	{
	// 		for (var x = 0; x < _levelData.GridData.GetLength(1); x++)
	// 		{
	// 			var cell = Board.Cells[x, y];
	// 				
	// 			var itemType = _levelData.GridData[x, y];
	// 			var item = ServiceProvider.GetItemFactory.CreateItem(itemType, Board.ItemsParent);
	// 			if (item == null) continue;					
	// 				 					
	// 			cell.Item = item;
	// 			item.transform.position = cell.transform.position;
	// 		}
	// 	}
	// }
}
