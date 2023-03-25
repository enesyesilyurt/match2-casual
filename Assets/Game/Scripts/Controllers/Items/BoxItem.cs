using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class BoxItem : Item
    {
        private int health = 2;
        
        public void PrepareBoxItem(ItemBase itemBase)
        {
            ItemType = ItemType.Box;
            var boxSprite = ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Box);
            Prepare(itemBase, boxSprite);
        }

        public override void Fall()
        {
            
        }

        protected override void Execute()
        {
            CellController.Item = null;
        }

        protected override void Prepare(ItemBase itemBase, Sprite sprite)
        {
            AddSprite(sprite);
        }

        public override void OnNeighbourExecute()
        {
            health--;
            if (health <= 0)
            {
                base.OnNeighbourExecute();
                RemoveItem();
            }
        }

        public override void ExecuteWithTapp()
        {
            
        }

        public override void ExecuteWithSpecial()
        {
            health--;
            if (health <= 0)
            {
                base.ExecuteWithSpecial();
                RemoveItem();
            }
        }
    }
}
