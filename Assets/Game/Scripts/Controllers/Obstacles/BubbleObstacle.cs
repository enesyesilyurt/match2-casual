using System.Collections;
using System.Collections.Generic;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class BubbleObstacle : Obstacle
    {
        public override void Prepare(CellController cell)
        {
            AddSprite(ImageLibrary.Instance.GetSprite(Colour.None, ItemType.Bubble));
        }
        
        public override void OnItemExecuted()
        {
            CellController.Obstacle = null;
            base.OnItemExecuted();
            SimplePool.Despawn(gameObject);
        }
    }
}
