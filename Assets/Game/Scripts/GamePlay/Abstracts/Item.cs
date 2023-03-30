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
    public abstract class Item
    {
        private FallAnimation fallAnimation;
        private CellController cellController;
        private ItemBase itemBase;

        public FallAnimation FallAnimation => fallAnimation;
        public ItemBase ItemBase => itemBase;

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
                    itemBase.gameObject.name = cellController.gameObject.name + " " + GetType().Name;
                }
            }
        }

        protected Item()
        {
            itemBase = SimplePool
                .Spawn(GameManager.Instance.ItemBasePrefab, Vector3.zero, Quaternion.Euler(Vector3.zero))
                .GetComponent<ItemBase>();
            itemBase.Prepare();
        }

        protected void Prepare(Sprite sprite)
        {
            fallAnimation = itemBase.FallAnimation;
            fallAnimation.Prepare(this);
            itemBase.AddSprite(sprite);
        }

        protected void PrepareRemove()
        {
            FallAnimation.PrepareRemove();
            LevelManager.Instance.ItemExecute(this);
        }

        protected void RemoveItem()
        {
            CellController.Item = null;
            SimplePool.Despawn(itemBase.gameObject);
        }
    }
}
