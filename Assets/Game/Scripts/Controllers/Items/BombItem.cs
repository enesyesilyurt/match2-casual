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
}
