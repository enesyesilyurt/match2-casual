using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Casual.Abstracts;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Casual.Controllers
{
    public class BoardController : MonoSingleton<BoardController>
    {
        public const int MinimumMatchCount = 2;
        private Vector2Int[] directions = new[]
        {
            Vector2Int.up, new Vector2Int(1,1),
            Vector2Int.right, new Vector2Int(1,-1),
            Vector2Int.down, new Vector2Int(-1,-1),
            Vector2Int.left, new Vector2Int(-1,1)
        };

        [SerializeField] private CellController cellControllerPrefab;
        [SerializeField] private Transform cellParent;
        [SerializeField] private Transform itemsParent;

        public CellController[] Cells;

        private MatchFinder matchFinder = new();
        private bool onAnim = false;
        private int cellCount;

        public Transform ItemsParent => itemsParent;
        public MatchFinder MatchFinder => matchFinder;
        private int rowCount => LevelManager.Instance.CurrentLevel.RowCount;
        private int ColumnCount => LevelManager.Instance.CurrentLevel.ColumnCount;
            
        public void Prepare()
        {
            Cells = new CellController[ColumnCount * rowCount];
            matchFinder.Setup();
            
            CreateCells();
            PrepareCells();
        }

        public void ResetBoard()
        {
            var tempItems = itemsParent.parent;
            Destroy(itemsParent.gameObject);
            var newItems = new GameObject();
            newItems.transform.SetParent(tempItems);
            newItems.name = "Items";
            itemsParent = newItems.transform;

            var temp = cellParent.parent;
            Destroy(cellParent.gameObject);
            var newParent = new GameObject();
            newParent.transform.SetParent(temp);
            newParent.name = "Cells";
            cellParent = newParent.transform;
        }
        
        private void CreateCells()
        {
            for (var column = 0; column < ColumnCount; column++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    if (LevelManager.Instance.CurrentLevel.Blocks[column * ColumnCount + row].ItemType == ItemType.None) continue;
                    cellCount++;
                    var cell = SimplePool.Spawn(cellControllerPrefab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<CellController>();
                    cell.transform.SetParent(cellParent);
                    Cells[column * ColumnCount + row] = cell;
                }
            }
        }
        
        private void PrepareCells()
        {
            for (var column = 0; column < ColumnCount; column++)
            {
                for (var row = 0; row < ColumnCount; row++)
                {
                    if (LevelManager.Instance.CurrentLevel.Blocks[column * ColumnCount + row].ItemType == ItemType.None) continue;
                    Cells[column * ColumnCount + row].Prepare(row, column, this);
                }
            }
        }
        
        public void CellTapped(CellController cellController)
        {
            if(onAnim) return;
            if (cellController == null) return;
            if (!cellController.HasItem()) return;
            if(cellController.Item.FallAnimation.IsFalling) return;
            var cells = matchFinder.FindMatches(cellController, cellController.Item.Colour);
            if (cellController.Item.ItemType == ItemType.BombItem)
            {
                cellController.Item.TryExecute();
            }
            else if (cells.Count < MinimumMatchCount) 
                FailMatchSequence(cellController.Item.transform);
            else if (cellController.Item.ItemType == ItemType.Cube) 
                ExplodeMatchingCells(cells);
            else 
                StartCoroutine(TappedSpecialItemRoutine(cellController, cells));
        }

        private void FailMatchSequence(Transform target)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(target.DORotate(Vector3.forward * 5, .1f).SetEase(Ease.OutSine));
            sequence.Append(target.DORotate(Vector3.forward * -5, .1f).SetEase(Ease.OutSine));
            sequence.Append(target.DORotate(Vector3.zero, .1f).SetEase(Ease.OutSine));
            sequence.OnComplete(() => target.rotation = quaternion.identity);
        }

        private IEnumerator TappedSpecialItemRoutine(CellController cell, List<CellController> cells)
        {
            onAnim = true;
            FallAndFillManager.Instance.StopFalls();
            var itemType = cell.Item.ItemType;
            for (var i = 0; i < cells.Count; i++)
            {
                var explodedCell = cells[i];
                var item = explodedCell.Item;
                item.IncreaseSortingOrder(rowCount);
                item.FallAnimation.PrepareRemove();
                item.transform.DOMove(cell.transform.position, GameManager.Instance.SpecialMergeTime)
                    .SetEase(Ease.InBack, GameManager.Instance.SpecialMergeOverShoot)
                    .OnComplete(() =>
                    {
                        item.TryExecute();
                        onAnim = false;
                    });
            }

            yield return new WaitForSeconds(GameManager.Instance.SpecialMergeTime + .1f);

            if (itemType == ItemType.Bomb)
            {
                CreateBomb(cell);
            }
            else if(itemType == ItemType.Rocket)
            {
                CreateRocket(cell);
            }
            else if(itemType == ItemType.Propeller)
            {
                CreatePropeller(cell);
            }
            FallAndFillManager.Instance.StartFalls();
        }

        private void CreateBomb(CellController cell)
        {
            cell.Item = ItemFactory.Instance.CreateItem(
                Colour.None, this.ItemsParent, ItemType.BombItem);
            cell.Item.transform.position = cell.transform.position;
            cell.Item.Fall();
        }

        private void CreateRocket(CellController cell)
        {
            cell.Item = ItemFactory.Instance.CreateItem(
                Colour.None, this.ItemsParent, ItemType.RocketItem);
            cell.Item.transform.position = cell.transform.position;
            cell.Item.Fall();
        }

        private void CreatePropeller(CellController cell)
        {
            cell.Item = ItemFactory.Instance.CreateItem(
                Colour.None, this.ItemsParent, ItemType.PropellerItem);
            cell.Item.transform.position = cell.transform.position;
            cell.Item.Fall();
        }
        
        private void ExplodeMatchingCells(List<CellController> cells)
        {
            for (var i = 0; i < cells.Count; i++)
            {
                var explodedCell = cells[i];
                var item = explodedCell.Item;
                item.FallAnimation.PrepareRemove();
                item.TryExecute();
            }
        }

        public void CheckMatches()
        {
            int totalMatchCount = 0;
            int counter = 0;
            for (var column = 0; column < ColumnCount; column++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    if (Cells[column * ColumnCount + row] == null) continue;
                    if(Cells[column * ColumnCount + row].Item == null) continue;
                    totalMatchCount += Cells[column * ColumnCount + row].Item.CheckMatches();
                    counter++;
                }
            }

            if (totalMatchCount <= 0 && counter == cellCount)
            {
                onAnim = true;
                StartCoroutine(ShuffleRoutine());
            }
        }

        private IEnumerator ShuffleRoutine()
        {
            yield return new WaitForSeconds(.7f);
            FallAndFillManager.Instance.StopFalls();

            List<Item> items = new();
            
            for (var coloumn = 0; coloumn < ColumnCount; coloumn++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    if(Cells[coloumn * ColumnCount + row] == null) continue;
                    items.Add(Cells[coloumn * ColumnCount + row].Item);
                    Cells[coloumn * ColumnCount + row].Item = null;
                }
            }
            
            for (var column = 0; column < ColumnCount; column++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    if(Cells[column * ColumnCount + row] == null) continue;
                    var index = Random.Range(0, items.Count);
                    Cells[column * ColumnCount + row].Item = items[index];
                    items.RemoveAt(index);
                    Cells[column * ColumnCount + row].Item.transform.DOMove(Cells[column * ColumnCount + row].transform.position, GameManager.Instance.ShuffleSpeed)
                        .SetEase(Ease.InBack);
                }
            }
            
            yield return new WaitForSeconds(GameManager.Instance.ShuffleSpeed);
            FallAndFillManager.Instance.StartFalls();
            onAnim = false;
            CheckMatches();
        }
        
        public CellController GetNeighbourWithDirection(CellController cellController, Direction direction)
        {
            var row = cellController.Row + directions[(int)direction].x;
            var column = cellController.Column + directions[(int)direction].y;

            if (row >= ColumnCount || row < 0 || column >= rowCount || column < 0) return null;
            if (LevelManager.Instance.CurrentLevel.Blocks[column * ColumnCount + row].ItemType == ItemType.None) return null;
            return Cells[column * ColumnCount + row];
        }
    }
}
