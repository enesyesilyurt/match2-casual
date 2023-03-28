using System.Collections;
using System.Collections.Generic;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class BushObstacle : Obstacle, IItemExecuteBlocker
    {
        private int health = 2;
        
        public override void Prepare(CellController cell)
        {
            AddSprite(ImageLibrary.Instance.GetSprite(Colour.None, ItemType.Bush));
        }

        public override void OnNeighbourExecute()
        {
            health--;
            if (health <= 0)
            {
                CellController.Obstacle = null;
                SimplePool.Despawn(gameObject);
            }
        }
    }
}
