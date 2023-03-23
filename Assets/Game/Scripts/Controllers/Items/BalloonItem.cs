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
            var sprite = ImageLibrary.Instance.GetSpecialSprite(ItemType.Balloon);
            Prepare(itemBase, sprite);
        }
    
        public override void ExecuteWithTapp()
        {
            
        }
    }
}
