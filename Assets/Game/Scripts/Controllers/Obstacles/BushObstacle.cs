using System.Collections;
using System.Collections.Generic;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class BushObstacle : Obstacle
    {
        private int health = 2;
        
        public override void Prepare(CellController cell)
        {
            cell.IsItemCanTap = false;
            cell.IsItemCanExecute = false;
            AddSprite(ImageLibrary.Instance.GetSprite(Colour.None, ItemType.Bush));
        }

        public override void OnNeighbourExecute()
        {
            health--;
            if (health <= 0)
            {
                CellController.IsItemCanTap = true;
                CellController.IsItemCanExecute = true;
                CellController.Obstacle = null;
                SimplePool.Despawn(gameObject);
            }
        }
    }
}
