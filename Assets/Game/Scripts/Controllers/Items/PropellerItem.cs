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
        var neighbors = new List<CellController>();
        neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.Down));
        neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.Up));
        neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.Right));
        neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.Left));
        
        base.TryExecute();

        foreach (var neighbor in neighbors)
        {
            if(neighbor != null && neighbor.HasItem())
                neighbor.Item.TryExecute();
        }
        neighbors.Clear();
    }
}
