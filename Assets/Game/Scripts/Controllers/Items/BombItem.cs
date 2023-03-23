using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

public class BombItem : Item
{
    public void PrepareBombItem(ItemBase itemBase)
    {
        ItemType = ItemType.BombItem;
        var bombSprite = ImageLibrary.Instance.GetSpecialSprite(ItemType.BombItem);
        Prepare(itemBase, bombSprite);
    }

    public override void TryExecute()
    {
        // var neighbors = new List<CellController>();
        // neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.Down));
        // neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.Up));
        // neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.Right));
        // neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.Left));
        // neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.UpLeft));
        // neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.UpRight));
        // neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.DownLeft));
        // neighbors.Add(BoardController.Instance.GetNeighbourWithDirection(CellController, Direction.DownRight));
        //
        // foreach (var neighbor in neighbors)
        // {
        //     if(neighbor != null && neighbor.HasItem())
        //         neighbor.Item.TryExecute();
        // }
        // neighbors.Clear();
        // base.TryExecute();
    }
}
