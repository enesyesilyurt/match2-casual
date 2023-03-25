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
        public void PrepareBalloonItem(ItemBase itemBase)
        {
            ItemType = ItemType.Balloon;
            var sprite = ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Balloon);
            Prepare(itemBase, sprite);
        }
    
        public override void ExecuteWithTapp()
        {
            
        }

        public override void ExecuteWithSpecial()
        {
            base.ExecuteWithSpecial();
            RemoveItem();
        }

        public override void OnNeighbourExecute()
        {
            base.OnNeighbourExecute();
            RemoveItem();
        }
    }
}
