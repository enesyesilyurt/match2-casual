using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual
{
    public class BoxItem : Item, IInitializableWithData, IExecutableWithNeighbor, IExecutableWithSpecial
    {
        private int health = 2;
        
        public void InitializeWithData(ItemData itemData, ItemBase itemBase)
        {
            ItemType = ItemType.Box;
            AddSprite(ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Box));
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
