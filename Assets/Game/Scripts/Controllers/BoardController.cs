using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Casual.Abstracts;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Casual.Controllers
{
    public class BoardController : MonoSingleton<BoardController>
    {
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
            
            foreach (Transform child in particleParent)
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
                    if (LevelManager.Instance.CurrentLevel.ItemDatas[y * gridWidth + x].ItemType == null) continue;
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
                if (Cells[i] == null) continue;
                Cells[i].Prepare();
            }
        }
        
        public void CellTapped(CellController cellController)
        {
            if(onAnim) return;
            if (!cellController.CanTap()) return;
            
            var executableWithTap = (IExecutableWithTap)cellController.Item;
            if (executableWithTap == null) return;
            
            executableWithTap.ExecuteWithTap();
            TargetManager.Instance.DecreaseMoveCount();
        }

        public void CheckMatches()
        {
            int totalColourMatchCount = 0;
            int counter = 0;
            for (var i = 0; i < gridSize; i++)
            {
                if (Cells[i] == null) continue;
                if(Cells[i].Item == null) continue;
                if(!Cells[i].Item.TryGetComponent<IMatchableWithColour>(out IMatchableWithColour matchable)) continue;
                var value = matchable.CheckMatchWithColour();
                totalColourMatchCount += value;
                counter++;
            }

            if (totalColourMatchCount <= 0 && counter == cellCount)
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

        public void CreatePropeller(CellController cell, float delay)
        {
            StartCoroutine(CreatePropellerRoutine(cell, delay));
        }

        private IEnumerator CreatePropellerRoutine(CellController cell, float delay)
        {
            yield return new WaitForSeconds(delay);
            CreatePropeller(cell);
            FallAndFillManager.Instance.Proccess();
            CheckMatches();
        }

        private void CreatePropeller(CellController cell)
        {
            var item = Item.SpawnItem(typeof(PropellerItem), cell.transform.position, out ItemBase itemBase);
            var initializableWithoutData = (IInitializableWithoutData)item;
            initializableWithoutData.InitializeWithoutData(itemBase);
            cell.Item = item;
        }

        public CellController GetCell(Vector2Int position)
        {
            if (position.y < 0 || position.x < 0 || position.y >= LevelManager.Instance.CurrentLevel.GridHeight 
                || position.x >= LevelManager.Instance.CurrentLevel.GridWidth) return null;
            
            return Cells[position.y * LevelManager.Instance.CurrentLevel.GridWidth + position.x];
        }
    }
}
