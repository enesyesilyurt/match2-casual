using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class BoxItem : Item, IInitializableWithData, IExecutableWithNeighbor, IExecutableWithSpecial, ITargetable<string>
    {
        private int health = 2;
        
        public string Value => nameof(BoxItem);
        
        public void InitializeWithData(ItemData itemData)
        {
            Prepare(ImageLibrary.Instance.GetSprite(nameof(BoxItem)));
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
