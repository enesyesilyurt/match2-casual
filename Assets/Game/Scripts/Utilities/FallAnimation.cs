using Casual.Abstracts;
using Casual.Controllers;
using Casual.Managers;
using DG.Tweening;
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
        private float maxSpeed => GameManager.Instance.CubeMaxSpeed;
        
        public void Prepare(Item item) => this.item = item;

        public void Update()
        {
            FallMovementHandler();
        }

        public void FallToTarget(CellController targetCellController)
        {
            if (this.targetCellController != null && targetCellController.Y >= this.targetCellController.Y) return;
            
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
        }

        private void FallMovementHandler()
        {
            if (!isFalling) return;
            
            velocity += GameManager.Instance.CubeAcceleration;
            velocity = velocity >= maxSpeed ? maxSpeed : velocity;
            var tempPosition = item.transform.position;
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

            item.transform.position = tempPosition;
        }

        private void JumpSequence()
        {
            jumpSequence = DOTween.Sequence(transform);
            jumpSequence.Append(transform.DOMoveY(.05f, .15f).SetRelative());
            jumpSequence.Append(transform.DOMoveY(-.05f, .2f).SetRelative().SetEase(Ease.OutBounce));
        }
    }
}
