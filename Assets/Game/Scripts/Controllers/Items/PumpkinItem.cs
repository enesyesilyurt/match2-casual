using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class PumpkinItem : Item, IInitializableWithData, IInitializableWithoutData, IExecutableWithNeighbor, IExecutableWithSpecial, IMovable
    {
        private int health = 2;
        
        public bool CanMove
        {
            get
            {
                return CellController.GetFirstCellBelow() != null &&
                       !CellController.GetFirstCellBelow().HasItem();
            }
        }

        public void InitializeWithData(ItemData itemData, ItemBase itemBase)
        {
            ItemType = ItemType.Pumpkin;
            Prepare(itemBase, ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Pumpkin));
        }

        public void InitializeWithoutData(ItemBase itemBase)
        {
            ItemType = ItemType.Pumpkin;
            Prepare(itemBase, ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Pumpkin));
        }
    
        public void Move()
        {
            FallAnimation.FallToTarget(CellController.GetFallTarget());
        }

        public void Execute()
        {
            health--;
            if (health <= 0)
            {
                PrepareExecute();
                RemoveItem();
            }
        }

        public void PrepareExecute()
        {
            PrepareRemove();
        }

        public void ExecuteWithSpecial()
        {
            Execute();
        }

        public void ExecuteWithNeighbor()
        {
            Execute();
        }
    }
}
