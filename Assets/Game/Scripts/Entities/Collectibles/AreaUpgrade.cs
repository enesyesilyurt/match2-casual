using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectibles/AreaUpgrade", fileName = "AreaUpgradeConfig")]
public class AreaUpgrade : CollectibleBase
{
    [SerializeField] private int maxCount;

    private bool isMaxed;
    
    public int MaxCount => maxCount;
    public bool IsMaxed => isMaxed;
    
    public override void ChangeValue(int amount)
    {
        if(value == maxCount && value + amount >= maxCount) return;

        value = maxCount <= amount + value ? maxCount : value + amount;
        isMaxed = value == maxCount;
        
        base.ChangeValue(value);
    }
}
