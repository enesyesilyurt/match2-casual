using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class PumpkinItem : Item
    {
        private int health = 2;
        
        public void PreparePumpkinItem(ItemBase itemBase)
        {
            ItemType = ItemType.Pumpkin;
            var pumpkinSprite = ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Pumpkin);
            Prepare(itemBase, pumpkinSprite);
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
