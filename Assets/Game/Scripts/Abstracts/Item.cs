using Casual.Controllers;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using UnityEngine;

namespace Casual.Abstracts
{
    public abstract class Item : MonoBehaviour
    {
        private const int BaseSortingOrder = 10;

        public FallAnimation FallAnimation;
        
        private CellController cellController;
        protected SpriteRenderer spriteRenderer;
        protected Colour colour;

        public Colour Colour => colour;
        
        public ItemType ItemType { get; protected set; }
        
        public CellController CellController
        {
            get => cellController;
            set
            {
                if (cellController == value) return;

                var oldCell = cellController;
                cellController = value;

                if (oldCell != null && oldCell.Item == this)
                {
                    oldCell.Item = null;
                }

                if (value != null)
                {
                    value.Item = this;
                    gameObject.name = cellController.gameObject.name + " " + GetType().Name;
                }
            }
        }

        protected void Prepare(ItemBase itemBase, Sprite sprite)
        {
            AddSprite(sprite);
            FallAnimation = itemBase.FallAnimation;
            FallAnimation.Prepare(this);
        }

        public void SetSortingOrder(int y)
        {
            spriteRenderer.sortingOrder = BaseSortingOrder + y;
        }

        public void IncreaseSortingOrder(int value)
        {
            spriteRenderer.sortingOrder += value;
        }

        private void AddSprite(Sprite sprite)
        {
            var tempRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer = tempRenderer == null
                ? gameObject.AddComponent<SpriteRenderer>()
                : spriteRenderer = tempRenderer;

            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = BaseSortingOrder;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        public int CheckMatches()
        {
            if (ItemType == ItemType.Balloon || ItemType == ItemType.Propeller) return 0;
            var matchCount = BoardController.Instance.MatchFinder.FindMatches(cellController, colour).Count;
            OnMatchCountChanged(matchCount);
            return matchCount - 1;
        }
        
        protected virtual void OnMatchCountChanged(int matchCount) { }
        
        public void Fall() => FallAnimation.FallToTarget(CellController.GetFallTarget());


        public virtual void ExecuteWithNeighbour() => Execute();
        
        public virtual void ExecuteWithTapp() => Execute();

        private void Execute()
        {
            FallAnimation.PrepareRemove();
            LevelManager.Instance.ItemExecute(this);
            RemoveItem();
        }

        private void RemoveItem() // TODO
        {
            CellController.Item = null;
            CellController = null;
            
            Destroy(gameObject.GetComponent<Item>());
            SimplePool.Despawn(gameObject);
        }
    }
}
