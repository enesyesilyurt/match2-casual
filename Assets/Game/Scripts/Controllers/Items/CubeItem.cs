using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace Casual.Controllers.Items
{
    public class CubeItem : Item, IInitializableWithData, IExecutableWithTap, IExecutableWithSpecial, IMatchableWithColour, IMovable
    {
        public static int MinimumMatchCount = 2;
        public static int PropellerSpawnCount = 6;

        public Colour Value => colour;
        
        public bool CanMove
        {
            get
            {
                return CellController.GetFirstCellBelow() != null &&
                       !CellController.GetFirstCellBelow().HasItem();
            }
        }
        
        public void InitializeWithData(ItemData itemData, ItemBase itemBase)
        {
            colour = itemData.Colour;
            ItemType = ItemType.Cube;
            Prepare(itemBase, ImageLibrary.Instance.GetSprite(colour));
        }

        public void PrepareExecute()
        {
            PrepareRemove();
            foreach (var neighbor in CellController.GetNeighbours())
            {
                if (neighbor != null && neighbor.HasItem())
                {
                    if (neighbor.Item.TryGetComponent<IExecutableWithNeighbor>(out IExecutableWithNeighbor executableWithNeighbor))
                    {
                        executableWithNeighbor.ExecuteWithNeighbor();
                    }
                }
            }
            
            var listener = (IItemExecuteListener)CellController.Obstacle;
            listener?.OnItemExecuted();
            
            // if(neighbor != null && neighbor.HasObstacle())
            //     neighbor.Obstacle.OnNeighbourExecute();
        }

        public void Execute()
        {
            CreateParticle();
            RemoveItem();
        }

        public void Move()
        {
            FallAnimation.FallToTarget(CellController.GetFallTarget());
        }

        public void ExecuteWithSpecial()
        {
            PrepareExecute();
            Execute();
        }

        public void ExecuteWithTap()
        {
            var cells = BoardController.Instance.MatchFinder.FindMatches(CellController, colour);
            if (cells.Count < MinimumMatchCount)
            {
                FailMatchSequence(transform);
            }
            else if (cells.Count < PropellerSpawnCount)
            {
                ExplodeMatchingCells(cells);
            }
            else
            {
                MultipleMergeAnim(CellController, cells);
            }
        }

        public void OnMatchCountChanged(int matchCount)
        {
            if (matchCount < GameManager.Instance.PropellerMatchCount)
            {
                spriteRenderer.sprite = ImageLibrary.Instance.GetSprite(colour);
                ItemType = ItemType.Cube;
            }
            else
            {
                spriteRenderer.sprite = ImageLibrary.Instance.GetSprite(colour, ItemType.MultipleCube);
                ItemType = ItemType.MultipleCube;
            }
        }

        private void CreateParticle()
        {
            var particle = SimplePool.Spawn(ParticleLibrary.Instance.GetParticle(colour).gameObject, transform.position,
                Quaternion.identity);
            particle.SetActive(true);
            particle.transform.SetParent(BoardController.Instance.ParticleParent);
        }
        
        private void ExplodeMatchingCells(List<CellController> cells)
        {
            FallAndFillManager.Instance.Proccess();
            for (var i = 0; i < cells.Count; i++)
            {
                var item = (IExecutable)cells[i].Item;
                item.Execute();
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
        
        private void MultipleMergeAnim(CellController cell, List<CellController> cells)
        {
            for (var i = 0; i < cells.Count; i++)
            {
                var item = cells[i].Item;
                var executable = (IExecutable)item;
                executable.PrepareExecute();
                item.IncreaseSortingOrder(LevelManager.Instance.CurrentLevel.GridHeight);
                item.transform.DOMove(cell.transform.position, GameManager.Instance.SpecialMergeTime)
                    .SetEase(Ease.InBack, GameManager.Instance.SpecialMergeOverShoot)
                    .OnComplete(() =>
                    {
                        executable.Execute();
                    });
            }
            BoardController.Instance.CreatePropeller(cell, GameManager.Instance.SpecialMergeTime);
        }

        public int CheckMatchWithColour()
        {
            var matchCountTemp = BoardController.Instance.MatchFinder.FindMatches(CellController, colour).Count;
            var matchCount = matchCountTemp - 1 <= 0 ? 0 : matchCountTemp - 1;
            OnMatchCountChanged(matchCount);
            return matchCount;
        }
    }
}
