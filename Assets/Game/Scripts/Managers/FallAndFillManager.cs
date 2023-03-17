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
        private bool isActive;
        
        private int rowCount => LevelManager.Instance.CurrentLevel.RowCount;
        private int coloumnCount => LevelManager.Instance.CurrentLevel.ColoumnCount;

        public void Init(BoardController boardController)
        {
            this.boardController = boardController;

            CreateFillingCells();
        }

        private void CreateFillingCells()
        {
            var cellList = new List<CellController>();
            for (var coloumn = 0; coloumn < coloumnCount; coloumn++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    var cell = boardController.Cells[coloumn * coloumnCount + row];
                    if (cell != null && cell.IsFillingCell)
                    {
                        cellList.Add(cell);
                    }
                }
            }

            fillingCells = cellList.ToArray();
        }

        public void StartFalls()
        {
            isActive = true;
            boardController.CheckMatches();
        }

        public void StopFalls()
        {
            isActive = false;
        }

        private void DoFalls()
        {
            for (var coloumn = 0; coloumn < coloumnCount; coloumn++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    var cell = boardController.Cells[coloumnCount * coloumn + row];
                    if (cell.Item != null && cell.FirstCellBelow != null && cell.FirstCellBelow.Item == null)
                    {
                        cell.Item.Fall();
                    }
                }
            }
        }

        private void DoFills()
        {
            for (var i = 0; i < fillingCells.Length; i++)
            {
                var cell = fillingCells[i];
                if (cell.Item == null)
                {
                    cell.Item = ItemFactory.Instance.CreateRandomItem(boardController.ItemsParent);
                    boardController.CheckMatches();

                    var offsetY = 0.0F;
                    var targetCellBelow = cell.GetFallTarget().FirstCellBelow;
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
        }

        public void Update()
        {
            if (!isActive) return;

            DoFalls();
            DoFills();
        }
    }
}
