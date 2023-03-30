using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class BalloonItem : Item, IInitializableWithData, IInitializableWithoutData, IExecutableWithNeighbor, IExecutableWithSpecial, IMovable, ITargetable<string>
    {
        public bool CanMove
        {
            get
            {
                return CellController.GetFirstCellBelow() != null &&
                       !CellController.GetFirstCellBelow().HasItem();
            }
        }
        
        public string Value => nameof(BalloonItem);
        
        public void InitializeWithData(ItemData itemData)
        {
            Prepare(ImageLibrary.Instance.GetSprite(nameof(BalloonItem)));
        }

        public void InitializeWithoutData()
        {
            Prepare(ImageLibrary.Instance.GetSprite(nameof(BalloonItem)));
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
