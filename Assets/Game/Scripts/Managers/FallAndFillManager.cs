using System;
using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Casual.Managers
{
    public class FallAndFillManager : MonoSingleton<FallAndFillManager>
    {
        private int totalColourRatio = 0;
        private Dictionary<Vector2, ItemData> itemRatios = new();
        private BoardController boardController;
        private CellController[] fillingCells;
        private int gridSize;

        public void Init(BoardController boardController)
        {
            this.boardController = boardController;
            gridSize = LevelManager.Instance.CurrentLevel.GridWidth * LevelManager.Instance.CurrentLevel.GridHeight;
            CreateFillingCells();
            PrepareRatios();
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

        private void DoFalls()
        {
            for (var i = 0; i < gridSize; i++)
            {
                var cell = boardController.Cells[i];
                if(cell == null) continue;
                
                var movable = cell.Item as IMovable;
                if (movable is { CanMove: true }) movable.Move();
            }
        }

        private void DoFills()
        {
            for (var i = 0; i < fillingCells.Length; i++)
            {
                var cell = fillingCells[i];
                while (cell.Item == null)
                {
                    cell.Item = CreateRandomItem(cell.transform.position);

                    var offsetY = 0.0F;
                    var targetCellBelow = cell.GetFallTarget().GetFirstCellBelow();
                    if (targetCellBelow != null)
                    {
                        if (targetCellBelow.Item != null)
                        {
                            offsetY = targetCellBelow.Item.ItemBase.transform.position.y + GameManager.Instance.OffsetY;
                        }
                    }

                    var cellPosition = cell.transform.position;
                    cellPosition.y += GameManager.Instance.OffsetY;
                    cellPosition.y = cellPosition.y > offsetY ? cellPosition.y : offsetY;

                    if (!cell.HasItem()) continue;
                    cell.Item.ItemBase.transform.position = cellPosition;
                    var movable = (IMovable)cell.Item;
                    movable.Move();
                }
            }
            boardController.CheckMatches();
        }

        private Item CreateRandomItem(Vector3 position)
        {
            var itemData = GetRandomDataWithRatio();
            var item = ItemFactory.GetItem(itemData.ItemType);
            
            var initializableWithData = (IInitializableWithData)item;
            initializableWithData.InitializeWithData(itemData);

            item.ItemBase.transform.position = position;
            
            return item;
        }

        public void Proccess()
        {
            StartCoroutine(ProccessRoutine());
        }

        private IEnumerator ProccessRoutine()
        {
            yield return null;
            DoFalls();
            DoFills();
        }

        private ItemData GetRandomDataWithRatio()
        {
            int value = Random.Range(0, totalColourRatio);
            foreach (var temp in itemRatios.Keys)
            {
                if(value > temp.y) continue;
                if(value < temp.x) continue;
                return itemRatios[temp];
            }

            return null;
        }

        private void PrepareRatios()
        {
            if(LevelManager.Instance.CurrentLevel.ItemRatios == null) return;
            itemRatios.Clear();
            totalColourRatio = 0;
            for (var i = 0; i < LevelManager.Instance.CurrentLevel.ItemRatios.Count; i++)
            {
                var colourRatio = LevelManager.Instance.CurrentLevel.ItemRatios[i];
                if(colourRatio.Ratio <= 0) continue;
                var oldIndex = totalColourRatio;
                totalColourRatio += colourRatio.Ratio;
                var newIndex = totalColourRatio - 1;
                itemRatios.Add(new Vector2(oldIndex, newIndex), colourRatio.ItemData);
            }
        }
    }
}
