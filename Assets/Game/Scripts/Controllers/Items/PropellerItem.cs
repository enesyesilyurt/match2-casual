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
        var bombSprite = ImageLibrary.Instance.GetSpecialSprite(ItemType.Propeller);
        Prepare(itemBase, bombSprite);
    }
    
    public override void ExecuteWithNeighbour()
    {
        foreach (var neighbor in CellController.GetNeighbours())
        {
            if(neighbor != null && neighbor.HasItem())
                neighbor.Item.ExecuteWithNeighbour();
        }
        base.ExecuteWithNeighbour();
    }
    
    public override void ExecuteWithTapp()
    {
        foreach (var neighbor in CellController.GetNeighbours())
        {
            if(neighbor != null && neighbor.HasItem())
                neighbor.Item.ExecuteWithNeighbour();
        }
        base.ExecuteWithTapp();
    }
}
