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
        ItemType = ItemType.Propeller;
        var bombSprite = ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Propeller);
        Prepare(itemBase, bombSprite);
    }
    
    public override void ExecuteWithNeighbour()
    {
        
    }
    
    public override void ExecuteWithTapp()
    {
        base.ExecuteWithTapp();
        foreach (var neighbor in CellController.GetNeighbours())
        {
            if(neighbor != null && neighbor.HasItem())
                neighbor.Item.ExecuteWithTapp();
        }
        RemoveItem();
    }
}
