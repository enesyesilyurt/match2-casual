using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class FallAndFillManager : MonoBehaviour
    {
        private bool _isActive;
        private BoardController _boardController;
        private LevelData _levelData;
        private CellController[] _fillingCells;

        public void Init(BoardController boardController, LevelData levelData)
        {
            _boardController = boardController;
            _levelData = levelData;

            CreateFillingCells();
        }
        
        private static readonly ItemType[] DefaultCubeArray = new[]
        {
            ItemType.GreenCube,
            ItemType.YellowCube,
            ItemType.BlueCube,
            ItemType.RedCube
        };

        protected static ItemType GetRandomCubeItemType()
        {
            return GetRandomItemTypeFromArray(DefaultCubeArray);
        }

        protected static ItemType GetRandomItemTypeFromArray(ItemType[] itemTypeArray)
        {
            return itemTypeArray[Random.Range(0, itemTypeArray.Length)];
        }

        private void CreateFillingCells()
        {
            var cellList = new List<CellController>();
            for (var y = 0; y < BoardController.Rows; y++)
            {
                for (var x = 0; x < BoardController.Cols; x++)
                {
                    var cell = _boardController.Cells[x, y];
                    if (cell != null && cell.IsFillingCell)
                    {
                        cellList.Add(cell);
                    }
                }
            }

            _fillingCells = cellList.ToArray();
        }

        public void StartFalls()
        {
            _isActive = true;
        }

        public void StopFalls()
        {
            _isActive = false;
        }

        private void DoFalls()
        {
            for (var y = 0; y < BoardController.Rows; y++)
            {
                for (var x = 0; x < BoardController.Cols; x++)
                {
                    var cell = _boardController.Cells[x, y];
                    if (cell.Item != null && cell.firstCellControllerBelow != null && cell.firstCellControllerBelow.Item == null)
                    {
                        cell.Item.Fall();
                    }
                }
            }
        }

        private void DoFills()
        {
            for (var i = 0; i < _fillingCells.Length; i++)
            {
                var cell = _fillingCells[i];
                if (cell.Item == null)
                {
                    cell.Item = ItemFactory.Instance.CreateItem(
                        GetRandomCubeItemType(), _boardController.ItemsParent);

                    var offsetY = 0.0F;
                    var targetCellBelow = cell.GetFallTarget().firstCellControllerBelow;
                    if (targetCellBelow != null)
                    {
                        if (targetCellBelow.Item != null)
                        {
                            offsetY = targetCellBelow.Item.transform.position.y + 1;
                        }
                    }

                    var p = cell.transform.position;
                    p.y += 2;
                    p.y = p.y > offsetY ? p.y : offsetY;

                    if (!cell.HasItem()) continue;
                    cell.Item.transform.position = p;
                    cell.Item.Fall();
                }
            }
        }

        public void Update()
        {
            if (!_isActive) return;

            DoFalls();
            DoFills();
        }
    }