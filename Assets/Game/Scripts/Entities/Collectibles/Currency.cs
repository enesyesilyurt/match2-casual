using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectibles/Currency", fileName = "CurrencyConfig")]
public class Currency : CollectibleBase
{
    public override void ChangeValue(int amount)
    {
        value += amount;
        base.ChangeValue(value);
    }
}