using System.Collections.Generic;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual.Managers
{
    public class FallAndFillManager : MonoSingleton<FallAndFillManager>
    {
        private BoardController boardController;
        private CellController[] fillingCells;
        private int gridSize;

        public void Init(BoardController boardController)
        {
            this.boardController = boardController;
            gridSize = LevelManager.Instance.CurrentLevel.GridWidth * LevelManager.Instance.CurrentLevel.GridHeight;
            CreateFillingCells();
        }

        public void ResetManager()
        {
            fillingCells = null;
        }

        private void CreateFillingCells()
        {
            var cellList = new List<CellController>();
            for (var i = 0; i < gridSize; i++)
            {
                var cell = boardController.Cells[i];
                if (cell != null && cell.IsFillingCell)
                {
                    cellList.Add(cell);
                }
            }

            fillingCells = cellList.ToArray();
        }

        public void StartFalls()
        {
            boardController.CheckMatches();
        }

        public void DoFalls()
        {
            for (var i = 0; i < gridSize; i++)
            {
                var cell = boardController.Cells[i];
                if(cell == null) continue;
                if (cell.HasItem() && cell.GetFirstCellBelow() != null && !cell.GetFirstCellBelow().HasItem())
                {
                    cell.Item.Fall();
                }
            }
        }

        public void DoFills()
        {
            for (var i = 0; i < fillingCells.Length; i++)
            {
                var cell = fillingCells[i];
                while (cell.Item == null)
                {
                    cell.Item = ItemFactory.Instance.CreateRandomItem(boardController.ItemsParent);

                    var offsetY = 0.0F;
                    var targetCellBelow = cell.GetFallTarget().GetFirstCellBelow();
                    if (targetCellBelow != null)
                    {
                        if (targetCellBelow.Item != null)
                        {
                            offsetY = targetCellBelow.Item.transform.position.y + GameManager.Instance.OffsetY;
                        }
                    }

                    var cellPosition = cell.transform.position;
                    cellPosition.y += GameManager.Instance.OffsetY;
                    cellPosition.y = cellPosition.y > offsetY ? cellPosition.y : offsetY;

                    if (!cell.HasItem()) continue;
                    cell.Item.transform.position = cellPosition;
                    cell.Item.Fall();
                }
            }
            boardController.CheckMatches();
        }
    }
}
