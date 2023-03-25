using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

public class PropellerItem : Item // TODO
{
    public void PreparePropellerItem(ItemBase itemBase)
    {
        ItemType = ItemType.Propeller;
        var bombSprite = ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Propeller);
        Prepare(itemBase, bombSprite);
    }
    
    public override void OnNeighbourExecute()
    {
        
    }

    public override void ExecuteWithSpecial()
    {
        base.ExecuteWithSpecial();
        foreach (var neighbor in CellController.GetNeighbours())
        {
            if (neighbor != null && neighbor.HasItem())
            {
                neighbor.Item.ExecuteWithSpecial();
            }
        }
        RemoveItem();
    }

    public override void ExecuteWithTapp()
    {
        base.ExecuteWithTapp();
        foreach (var neighbor in CellController.GetNeighbours())
        {
            if (neighbor != null && neighbor.HasItem())
            {
                neighbor.Item.ExecuteWithSpecial();
            }
        }
        RemoveItem();
    }
}
