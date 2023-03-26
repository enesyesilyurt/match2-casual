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
        
        public override void Prepare(ItemBase itemBase, Colour colour)
        {
            ItemType = ItemType.Pumpkin;
            var pumpkinSprite = ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Pumpkin);
            AddSprite(pumpkinSprite);
            base.Prepare(itemBase, colour);
        }

        public override void OnNeighbourExecute()
        {
            if(!CellController.IsItemCanExecute) return;
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
            if(!CellController.IsItemCanExecute) return;
            health--;
            if (health <= 0)
            {
                base.ExecuteWithSpecial();
                RemoveItem();
            }
        }
    }
}
