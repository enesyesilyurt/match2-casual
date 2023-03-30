using Casual.Abstracts;
using Casual.Controllers;
using Casual.Managers;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Casual.Utilities
{
    public class FallAnimation : MonoBehaviour
    {
        private static float startVelocity = 0F;
        
        private Item item;
        private CellController targetCellController;
        private Sequence jumpSequence;
        private Vector3 targetPosition;
        private float velocity = startVelocity;
        private bool isFalling;

        public bool IsFalling => isFalling;
        private bool canMovable = true;
        private float maxSpeed;
        
        public void Prepare(Item item)
        {
            this.item = item;
            var movable = item as IMovable;
            if (movable == null)
            {
                canMovable = false;
            }
            else
            {
                maxSpeed = GameManager.Instance.CubeMaxSpeed;
            }
        }

        public void Update()
        {
            if(canMovable && !isFalling) return;
            FallMovementHandler();
        }

        public void FallToTarget(CellController targetCellController)
        {
            if (item != null && this.targetCellController != null && targetCellController.GridPosition.y >= this.targetCellController.GridPosition.y) return;
            
            if(jumpSequence != null)
                jumpSequence.Kill();
            
            this.targetCellController = targetCellController;
            item.CellController = this.targetCellController;
            targetPosition = this.targetCellController.transform.position;
            isFalling = true;
        }

        public void PrepareRemove()
        {
            if(jumpSequence != null)
                jumpSequence.Kill();
            StopFalling();
        }

        public void StopFalling()
        {
            isFalling = false;
        }
        
        private void FallMovementHandler()
        {
            velocity += GameManager.Instance.CubeAcceleration;
            velocity = velocity >= maxSpeed ? maxSpeed : velocity;
            var tempPosition = item.ItemBase.transform.position;
            tempPosition.y -= velocity * Time.deltaTime;
            if (tempPosition.y <= targetPosition.y)
            {
                targetCellController = null;
                tempPosition.y = targetPosition.y;
                velocity = startVelocity;
                isFalling = false;
                
                if (jumpSequence == null) JumpSequence();
                else
                {
                    if (!jumpSequence.IsActive()) JumpSequence();
                }
            }
            
            item.ItemBase.transform.position = tempPosition;
            
            // item.transform.DOKill();
            //
            // item.transform.DOMoveY(targetCellController.transform.position.y, 10)
            //     .SetSpeedBased().SetEase(Ease.InSine).OnComplete(() =>
            //     {
            //         if (jumpSequence == null) JumpSequence();
            //         else
            //         {
            //             if (!jumpSequence.IsActive()) JumpSequence();
            //         }
            //         isFalling = false;
            //         targetCellController = null;
            //     });
        }

        private void JumpSequence()
        {
            jumpSequence = DOTween.Sequence(transform);
            jumpSequence.Append(transform.DOMoveY(.05f, .15f).SetRelative());
            jumpSequence.Append(transform.DOMoveY(-.05f, .2f).SetRelative().SetEase(Ease.OutBounce));
        }
    }
}
