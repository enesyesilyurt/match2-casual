using Casual.Controllers;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Casual.Abstracts
{
    public abstract class Item : MonoBehaviour
    {
        private const int BaseSortingOrder = 10;

        public FallAnimation FallAnimation;
        
        private CellController cellController;
        private SpriteRenderer spriteRenderer;
        protected Colour colour;

        public Colour Colour => colour;
        
        public ItemType ItemType { get; protected set; }
        
        public CellController CellController
        {
            get { return cellController; }
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
            var tempRenderer = GetComponentInChildren<SpriteRenderer>();
            if (tempRenderer == null)
            {
                spriteRenderer = new GameObject("Sprite_").AddComponent<SpriteRenderer>();
                spriteRenderer.transform.SetParent(transform);
            }
            else
            {
                spriteRenderer = tempRenderer;
            }
            
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = BaseSortingOrder;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        public int CheckMatches()
        {
            if (ItemType == ItemType.BombItem || ItemType == ItemType.RocketItem || ItemType == ItemType.PropellerItem) return 0;
            var matchCount = BoardController.Instance.MatchFinder.FindMatches(cellController, colour).Count;
            SetSprite(matchCount);
            return matchCount - 1;
        }

        private void SetSprite(int matchCount)
        {
            if (matchCount < GameManager.Instance.PropellerMatchCount)
            {
                spriteRenderer.sprite = ImageLibrary.Instance.GetSprite(colour);
                ItemType = ItemType.Cube;
            }
            else
            {
                spriteRenderer.sprite = ImageLibrary.Instance.GetSprite(colour, ItemType.Propeller);
                ItemType = ItemType.Propeller;
            }
        }
        
        public void Fall()
        {
            FallAnimation.FallToTarget(CellController.GetFallTarget());
        }

        public virtual void TryExecute()
        {
            LevelManager.Instance.ItemExecute(this);
            RemoveItem();
        }

        private void RemoveItem()
        {
            CellController.Item = null;
            CellController = null;
            
            Destroy(gameObject.GetComponent<Item>());
            SimplePool.Despawn(gameObject);
        }
    }
}
