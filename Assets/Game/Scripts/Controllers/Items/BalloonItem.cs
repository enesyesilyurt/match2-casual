using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class BalloonItem : Item
    {
        public override void Prepare(ItemBase itemBase, Colour colour)
        {
            ItemType = ItemType.Balloon;
            var sprite = ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Balloon);
            AddSprite(sprite);
            base.Prepare(itemBase, colour);
        }
    
        public override void ExecuteWithTapp()
        {
            
        }

        public override void ExecuteWithSpecial()
        {
            if(!CellController.IsItemCanExecute) return;
            base.ExecuteWithSpecial();
            RemoveItem();
        }

        public override void OnNeighbourExecute()
        {
            if(!CellController.IsItemCanExecute) return;
            base.OnNeighbourExecute();
            RemoveItem();
        }
    }
}
