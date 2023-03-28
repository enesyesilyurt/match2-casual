using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class BalloonItem : Item, IInitializableWithData, IInitializableWithoutData, IExecutableWithNeighbor, IExecutableWithSpecial, IMovable
    {
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
            ItemType = ItemType.Balloon;
            Prepare(itemBase, ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Balloon));
        }

        public void InitializeWithoutData(ItemBase itemBase)
        {
            ItemType = ItemType.Balloon;
            Prepare(itemBase, ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Balloon));
        }
    
        public void Move()
        {
            FallAnimation.FallToTarget(CellController.GetFallTarget());
        }

        public void Execute()
        {
            PrepareExecute();
            RemoveItem();
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
