using System.Collections;
using System.Collections.Generic;
using Casual.Utilities;
using UnityEngine;

public class ItemFactory : MonoSingleton<ItemFactory>
{
    [SerializeField] private ItemBase ItemBasePrefab;
    
    public Item CreateItem(ItemType itemType, Transform parent)
        {
            if (itemType == ItemType.None)
            {
                return null;
            }

            var itemBase = Instantiate(ItemBasePrefab, Vector3.zero, Quaternion.identity, parent);

            Item item = null;
            switch (itemType)
            {
                case ItemType.GreenCube:
                    item = CreateCubeItem(itemBase, MatchType.Green);
                    break;
                case ItemType.YellowCube:
                    item = CreateCubeItem(itemBase, MatchType.Yellow);
                    break;
                case ItemType.BlueCube:
                    item = CreateCubeItem(itemBase, MatchType.Blue);
                    break;
                case ItemType.RedCube:
                    item = CreateCubeItem(itemBase, MatchType.Red);
                    break;
                default:
                    Debug.LogWarning("Can not create item: " + itemType);
                    break;
            }

            return item;
        }
    
    private Item CreateCubeItem(ItemBase itemBase, MatchType matchType)
    {
        var cubeItem = itemBase.gameObject.AddComponent<CubeItem>();
        cubeItem.PrepareCubeItem(itemBase, matchType);

        return cubeItem;
    }
}
