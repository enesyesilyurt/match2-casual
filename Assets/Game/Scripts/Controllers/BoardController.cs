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

        public CellController[,] Cells;

        private MatchFinder matchFinder = new();
        private bool onAnim = false;

        public Transform ItemsParent => itemsParent;
        public MatchFinder MatchFinder => matchFinder;
        private int rowCount => LevelManager.Instance.CurrentLevel.Size.y;
        private int ColoumnCount => LevelManager.Instance.CurrentLevel.Size.x;
            
        public void Prepare()
        {
            Cells = new CellController[ColoumnCount, rowCount];
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
            for (var y = 0; y < rowCount; y++)
            {
                for (var x = 0; x < ColoumnCount; x++)
                {
                    var cell = Instantiate(cellControllerPrefab, Vector3.zero, Quaternion.identity, cellParent);
                    Cells[x, y] = cell;
                }
            }
        }
        
        private void PrepareCells()
        {
            for (var y = 0; y < rowCount; y++)
            {
                for (var x = 0; x < ColoumnCount; x++)
                {
                    Cells[x, y].Prepare(x, y, this);
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
            
            if (cells.Count < MinimumMatchCount) 
                FailMatchSequence(cellController.Item.transform);
            else if (cellController.Item.ItemType == ItemType.Default) 
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
                        item.RemoveItem();
                        onAnim = false;
                    });
            }

            yield return new WaitForSeconds(GameManager.Instance.SpecialMergeTime + .1f);
            CreateBomb(cell);
            FallAndFillManager.Instance.StartFalls();
        }

        private void CreateBomb(CellController cell)
        {
            cell.Item = ItemFactory.Instance.CreateItem(
                Colour.None, this.ItemsParent, ItemType.BombItem);
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
                item.RemoveItem();
            }
        }

        public void CheckMatches()
        {
            int totalMatchCount = 0;
            int counter = 0;
            for (var y = 0; y < rowCount; y++)
            {
                for (var x = 0; x < ColoumnCount; x++)
                {
                    if (Cells[x, y] == null) return;
                    if(Cells[x, y].Item == null) return;
                    totalMatchCount += Cells[x, y].Item.CheckMatches();
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
            
            for (var y = 0; y < rowCount; y++)
            {
                for (var x = 0; x < ColoumnCount; x++)
                {
                    items.Add(Cells[x, y].Item);
                    Cells[x, y].Item = null;
                }
            }
            
            for (var y = 0; y < rowCount; y++)
            {
                for (var x = 0; x < ColoumnCount; x++)
                {
                    var index = Random.Range(0, items.Count);
                    Cells[x, y].Item = items[index];
                    items.RemoveAt(index);
                    Cells[x, y].Item.transform.DOMove(Cells[x, y].transform.position, GameManager.Instance.ShuffleSpeed)
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
            var x = cellController.X;
            var y = cellController.Y;
            switch (direction)
            {
                case Direction.None:
                    break;
                case Direction.Up:
                    y += 1;
                    break;
                case Direction.UpRight:
                    y += 1;
                    x += 1;
                    break;
                case Direction.Right:
                    x += 1;
                    break;
                case Direction.DownRight:
                    y -= 1;
                    x += 1;
                    break;
                case Direction.Down:
                    y -= 1;
                    break;
                case Direction.DownLeft:
                    y -= 1;
                    x -= 1;
                    break;
                case Direction.Left:
                    x -= 1;
                    break;
                case Direction.UpLeft:
                    y += 1;
                    x -= 1;
                    break;
            }

            if (x >= ColoumnCount || x < 0 || y >= rowCount || y < 0) return null;

            return Cells[x, y];
        }
    }
}
