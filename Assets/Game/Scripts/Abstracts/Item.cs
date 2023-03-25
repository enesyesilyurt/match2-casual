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

        protected virtual void Prepare(ItemBase itemBase, Sprite sprite)
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

        protected void AddSprite(Sprite sprite)
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
        
        public virtual void Fall() => FallAnimation.FallToTarget(CellController.GetFallTarget());


        public virtual void OnNeighbourExecute() => Execute();
        
        public virtual void ExecuteWithTapp() => Execute();
        public virtual void ExecuteWithSpecial() => Execute();

        protected virtual void Execute()
        {
            FallAnimation.PrepareRemove();
            CellController.Item = null;
        }

        protected void RemoveItem() // TODO
        {
            CellController = null;
            LevelManager.Instance.ItemExecute(this);
            Destroy(gameObject.GetComponent<Item>());
            SimplePool.Despawn(gameObject);
        }
    }
}
