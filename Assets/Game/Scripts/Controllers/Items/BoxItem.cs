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
        
        public override void Prepare(ItemBase itemBase, Colour colour)
        {
            ItemType = ItemType.Box;
            var boxSprite = ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Box);
            AddSprite(boxSprite);
        }

        public override void Fall()
        {
            
        }

        protected override void Execute()
        {
            CellController.Item = null;
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
