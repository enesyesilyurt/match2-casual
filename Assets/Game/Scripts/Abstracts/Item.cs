using Casual.Controllers;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using UnityEditor;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace Casual.Abstracts
{
    public abstract class Item : MonoBehaviour
    {
        private const int BaseSortingOrder = 10;

        private FallAnimation fallAnimation;
        private CellController cellController;
        protected SpriteRenderer spriteRenderer;
        protected Colour colour;

        public FallAnimation FallAnimation => fallAnimation;
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

        public static Item SpawnItem(Type type, Vector3 position, out ItemBase itemBase)
        {
            itemBase = SimplePool
                .Spawn(GameManager.Instance.ItemBasePrefab, position, Quaternion.Euler(Vector3.zero))
                .GetComponent<ItemBase>();
            itemBase.Prepare();
            return (Item)itemBase.gameObject.AddComponent(type);
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
            if (ItemType != ItemType.Cube && ItemType != ItemType.MultipleCube) return 0;
            var matchCountTemp = BoardController.Instance.MatchFinder.FindMatches(cellController, colour).Count;
            var matchCount = matchCountTemp - 1 <= 0 ? 0 : matchCountTemp - 1;
            OnMatchCountChanged(matchCount);
            return matchCount;
        }
        
        protected virtual void OnMatchCountChanged(int matchCount) { }

        protected void Prepare(ItemBase itemBase, Sprite sprite)
        {
            fallAnimation = itemBase.FallAnimation;
            fallAnimation.Prepare(this);
            AddSprite(sprite);
        }

        protected void PrepareRemove()
        {
            CellController.Item = null;
            LevelManager.Instance.ItemExecute(this);
        }

        protected void RemoveItem()
        {
            Destroy(gameObject.GetComponent<Item>());
            SimplePool.Despawn(gameObject);
        }
    }
}
