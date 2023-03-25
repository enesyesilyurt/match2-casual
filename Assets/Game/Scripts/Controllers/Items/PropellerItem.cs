using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

public class PropellerItem : Item // TODO
{
    public override void Prepare(ItemBase itemBase, Colour colour)
    {
        ItemType = ItemType.Propeller;
        var propellerSprite = ImageLibrary.Instance.GetSprite(Colour.Empty, ItemType.Propeller);
        AddSprite(propellerSprite);
        base.Prepare(itemBase, colour);
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
