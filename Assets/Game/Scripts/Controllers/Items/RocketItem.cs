using System.Collections;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

public class RocketItem : Item
{
    public void PrepareRocketItem(ItemBase itemBase)
    {
        ItemType = ItemType.RocketItem;
        var bombSprite = ImageLibrary.Instance.GetSpecialSprite(ItemType.RocketItem);
        Prepare(itemBase, bombSprite);
    }
}
