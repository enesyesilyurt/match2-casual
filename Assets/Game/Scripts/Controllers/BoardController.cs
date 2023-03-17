using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private CellController cellControllerPrefab;
        [SerializeField] private Transform cellParent;
        [SerializeField] private Transform itemsParent;
        [SerializeField] private Transform spriteMaskTransform;
        [SerializeField] private SpriteRenderer borderSprite;
        [SerializeField] private SpriteRenderer borderSprite2;

        public CellController[] Cells;

        private MatchFinder matchFinder = new();
        private bool onAnim = false;

        public Transform ItemsParent => itemsParent;
        public MatchFinder MatchFinder => matchFinder;
        private int rowCount => LevelManager.Instance.CurrentLevel.RowCount;
        private int ColoumnCount => LevelManager.Instance.CurrentLevel.ColoumnCount;
            
        public void Prepare()
        {
            Cells = new CellController[ColoumnCount * rowCount];
            matchFinder.Setup();
            
            SetBoardElements();
            CreateCells();
            PrepareCells();
        }

        private void SetBoardElements()
        {
            var boardSize = new Vector2(
                ColoumnCount * GameManager.Instance.OffsetX + .15f, 
                rowCount * GameManager.Instance.OffsetY + .15f);
            var boardPosition =
                Vector3.left * (ColoumnCount * GameManager.Instance.OffsetX / 2f - GameManager.Instance.OffsetX / 2f) +
                Vector3.down * (rowCount * GameManager.Instance.OffsetY / 2f - GameManager.Instance.OffsetY / 2f);
            
            borderSprite.size = boardSize;
            borderSprite.transform.position -= boardPosition;
            
            borderSprite2.size = boardSize;
            borderSprite2.transform.position -= boardPosition;
            
            spriteMaskTransform.localScale = boardSize;
            spriteMaskTransform.transform.position -= boardPosition;
        }
        
        private void CreateCells()
        {
            for (var coloumn = 0; coloumn < ColoumnCount; coloumn++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    var cell = Instantiate(cellControllerPrefab, Vector3.zero, Quaternion.identity, cellParent);
                    Cells[coloumn * ColoumnCount + row] = cell;
                }
            }
        }
        
        private void PrepareCells()
        {
            for (var coloumn = 0; coloumn < ColoumnCount; coloumn++)
            {
                for (var row = 0; row < ColoumnCount; row++)
                {
                    Cells[coloumn * ColoumnCount + row].Prepare(row, coloumn, this);
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
            for (var coloumn = 0; coloumn < ColoumnCount; coloumn++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    if (Cells[coloumn * ColoumnCount + row] == null) return;
                    if(Cells[coloumn * ColoumnCount + row].Item == null) return;
                    totalMatchCount += Cells[coloumn * ColoumnCount + row].Item.CheckMatches();
                    counter++;
                }
            }

            if (totalMatchCount <= 0 && counter == rowCount * ColoumnCount)
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
            
            for (var coloumn = 0; coloumn < ColoumnCount; coloumn++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    items.Add(Cells[coloumn * ColoumnCount + row].Item);
                    Cells[coloumn * ColoumnCount + row].Item = null;
                }
            }
            
            for (var coloumn = 0; coloumn < ColoumnCount; coloumn++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    var index = Random.Range(0, items.Count);
                    Cells[coloumn * ColoumnCount + row].Item = items[index];
                    items.RemoveAt(index);
                    Cells[coloumn * ColoumnCount + row].Item.transform.DOMove(Cells[coloumn * ColoumnCount + row].transform.position, GameManager.Instance.ShuffleSpeed)
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
            var row = cellController.X;
            var coloumn = cellController.Y;
            switch (direction)
            {
                case Direction.None:
                    break;
                case Direction.Up:
                    coloumn += 1;
                    break;
                case Direction.UpRight:
                    coloumn += 1;
                    row += 1;
                    break;
                case Direction.Right:
                    row += 1;
                    break;
                case Direction.DownRight:
                    coloumn -= 1;
                    row += 1;
                    break;
                case Direction.Down:
                    coloumn -= 1;
                    break;
                case Direction.DownLeft:
                    coloumn -= 1;
                    row -= 1;
                    break;
                case Direction.Left:
                    row -= 1;
                    break;
                case Direction.UpLeft:
                    coloumn += 1;
                    row -= 1;
                    break;
            }

            if (row >= ColoumnCount || row < 0 || coloumn >= rowCount || coloumn < 0) return null;

            return Cells[coloumn * ColoumnCount + row];
        }
    }
}
