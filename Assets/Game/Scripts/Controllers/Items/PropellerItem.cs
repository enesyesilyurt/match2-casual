using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

public class PropellerItem : Item
{
    public void PreparePropellerItem(ItemBase itemBase)
    {
        ItemType = ItemType.PropellerItem;
        var bombSprite = ImageLibrary.Instance.GetSpecialSprite(ItemType.PropellerItem);
        Prepare(itemBase, bombSprite);
    }
    
    public override void TryExecute()
    {
        foreach (var neighbor in CellController.GetNeighbours())
        {
            if(neighbor != null && neighbor.HasItem())
                neighbor.Item.TryExecute();
        }
        base.TryExecute();
    }
}
