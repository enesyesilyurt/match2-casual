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
        public static Vector2Int[] directions = new[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left, 
            new Vector2Int(1,1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,-1),
            new Vector2Int(-1,1)
        };

        [SerializeField] private CellController cellControllerPrefab;
        [SerializeField] private Transform cellParent;
        [SerializeField] private Transform itemsParent;
        [SerializeField] private Transform particleParent;

        public CellController[] Cells;

        private MatchFinder matchFinder = new();
        private bool onAnim = false;
        private int cellCount;
        private int gridSize;

        public Transform ItemsParent => itemsParent;
        public Transform ParticleParent => particleParent;
        public MatchFinder MatchFinder => matchFinder;
            
        public void Prepare()
        {
            onAnim = true;
            gridSize = LevelManager.Instance.CurrentLevel.GridWidth * LevelManager.Instance.CurrentLevel.GridHeight;
            
            Cells = new CellController[gridSize];
            matchFinder.Setup();
            
            CreateCells();
            PrepareCells();

            transform.position = Vector3.right * 10;
            transform.DOMoveX(0, .5f).SetEase(Ease.OutBack).OnComplete(()=> onAnim = false).SetDelay(.1f);
        }

        public void ResetBoard()
        {
            foreach (Transform child in itemsParent)
            {
                child.DOKill();
                SimplePool.Despawn(child.gameObject);
            }
            
            foreach (Transform child in cellParent)
            {
                child.DOKill();
                SimplePool.Despawn(child.gameObject);
            }
        }
        
        private void CreateCells()
        {
            var gridWidth = LevelManager.Instance.CurrentLevel.GridWidth;
            
            for (var x = 0; x < gridWidth; x++)
            {
                for (var y = 0; y < LevelManager.Instance.CurrentLevel.GridHeight; y++)
                {
                    if (LevelManager.Instance.CurrentLevel.Blocks[y * gridWidth + x].ItemType == ItemType.None) continue;
                    cellCount++;
                    var cell = SimplePool.Spawn(cellControllerPrefab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<CellController>();
                    Cells[y * gridWidth + x] = cell;
                    cell.Initialize(x,y, cellParent);
                }
            }
        }
        
        private void PrepareCells()
        {
            for (var i = 0; i < gridSize; i++)
            {
                if (LevelManager.Instance.CurrentLevel.Blocks[i].ItemType == ItemType.None) continue;
                Cells[i].Prepare();
            }
        }
        
        public void CellTapped(CellController cellController)
        {
            if(onAnim) return;
            if (!cellController.HasItem()) return;
            if(cellController.Item.FallAnimation.IsFalling) return;
            var cells = matchFinder.FindMatches(cellController, cellController.Item.Colour);
            if (cellController.Item.ItemType == ItemType.Propeller)
            {
                TargetManager.Instance.DecreaseMoveCount();
                cellController.Item.ExecuteWithTapp();
                
                FallAndFillManager.Instance.DoFalls();
                FallAndFillManager.Instance.DoFills();
            }
            else if (cells.Count < MinimumMatchCount) 
                FailMatchSequence(cellController.Item.transform);
            else if (cellController.Item.ItemType == ItemType.Cube)
            {
                ExplodeMatchingCells(cells);
                FallAndFillManager.Instance.DoFalls();
                FallAndFillManager.Instance.DoFills();
                TargetManager.Instance.DecreaseMoveCount();
            }
            else
            {
                StartCoroutine(TappedSpecialItemRoutine(cellController, cells));
                TargetManager.Instance.DecreaseMoveCount();
            }
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
            var itemType = cell.Item.ItemType;
            for (var i = 0; i < cells.Count; i++)
            {
                var explodedCell = cells[i];
                var item = explodedCell.Item;
                item.IncreaseSortingOrder(LevelManager.Instance.CurrentLevel.GridHeight);
                item.FallAnimation.PrepareRemove();
                item.transform.DOMove(cell.transform.position, GameManager.Instance.SpecialMergeTime)
                    .SetEase(Ease.InBack, GameManager.Instance.SpecialMergeOverShoot)
                    .OnComplete(() =>
                    {
                        item.ExecuteWithTapp();
                        onAnim = false;
                    });
            }

            yield return new WaitForSeconds(GameManager.Instance.SpecialMergeTime + .1f);
            
            if(itemType == ItemType.MultipleCube)
            {
                CreatePropeller(cell);
            }
            FallAndFillManager.Instance.DoFalls();
            FallAndFillManager.Instance.DoFills();
        }

        private void CreatePropeller(CellController cell)
        {
            cell.Item = ItemFactory.Instance.CreateItem(
                Colour.None, this.ItemsParent, ItemType.Propeller);
            cell.Item.transform.position = cell.transform.position;
            cell.Item.Fall();
        }
        
        private void ExplodeMatchingCells(List<CellController> cells)
        {
            for (var i = 0; i < cells.Count; i++)
            {
                cells[i].Item.ExecuteWithTapp();
            }
        }

        public void CheckMatches()
        {
            int totalMatchCount = 0;
            int counter = 0;
            for (var i = 0; i < gridSize; i++)
            {
                if (Cells[i] == null) continue;
                if(Cells[i].Item == null) continue;
                totalMatchCount += Cells[i].Item.CheckMatches();
                counter++;
                
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

            List<Item> items = new();
            
            for (var i = 0; i < gridSize; i++)
            {
                if(Cells[i] == null) continue;
                items.Add(Cells[i].Item);
                Cells[i].Item = null;
            }

            for (var i = 0; i < gridSize; i++)
            {
                if(Cells[i] == null) continue;
                var index = Random.Range(0, items.Count);
                Cells[i].Item = items[index];
                items.RemoveAt(index);
                Cells[i].Item.transform.DOMove(Cells[i].transform.position, GameManager.Instance.ShuffleSpeed)
                    .SetEase(Ease.InBack);
                
            }
            
            yield return new WaitForSeconds(GameManager.Instance.ShuffleSpeed);
            onAnim = false;
            CheckMatches();
        }

        public CellController GetCell(Vector2Int position)
        {
            if (position.y * LevelManager.Instance.CurrentLevel.GridWidth + position.x >
                LevelManager.Instance.CurrentLevel.GridWidth * LevelManager.Instance.CurrentLevel.GridHeight)
                return null;
            return Cells[position.y * LevelManager.Instance.CurrentLevel.GridWidth + position.x];
        }
    }
}
