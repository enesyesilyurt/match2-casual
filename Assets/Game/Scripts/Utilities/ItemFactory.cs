using System;
using System.Collections.Generic;
using Casual.Abstracts;
using Casual.Controllers;
using Casual.Controllers.Items;
using Casual.Enums;
using Casual.Managers;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Casual.Utilities
{
    public class ItemFactory : MonoSingleton<ItemFactory>
    {
        [SerializeField] private ItemBase itemBasePrefab;

        private Colour[] colours =
        {
            Colour.Green, Colour.Yellow, Colour.Blue, 
            Colour.Red, Colour.Pink, Colour.Purple
        };

        private int totalColourRatio = 0;

        private Dictionary<ItemType, Type> itemDict = new ();
        private Dictionary<Vector2, Colour> colourRatioDict = new();

        public void Setup()
        {
            PrepareRatios();
        }

        public void Initialize()
        {
            PrepareItemTypeDict();
        }

        private void PrepareItemTypeDict()
        {
            itemDict.Add(ItemType.Cube, typeof(CubeItem));
            itemDict.Add(ItemType.Propeller, typeof(PropellerItem));
            itemDict.Add(ItemType.Balloon, typeof(BalloonItem));
            itemDict.Add(ItemType.Box, typeof(BoxItem));
            itemDict.Add(ItemType.Pumpkin, typeof(PumpkinItem));
        }
        
        public Item CreateItem(Colour colour, Transform parent, ItemType itemType = ItemType.Cube) // TODO
        {
            if (itemType == ItemType.Cube && (colour == Colour.Empty || colour == Colour.None)) return null;
            var itemBase = SimplePool.Spawn(itemBasePrefab.gameObject, Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<ItemBase>();
            itemBase.gameObject.SetActive(true);
            itemBase.transform.parent = parent;
            Item item = itemDict.ContainsKey(itemType) ? CreateItemWithType(itemBase, itemDict[itemType], colour) : null;

            return item;
        }

        public Item CreateRandomItem(Transform parent)
        {
            var colour = GetRandomColourWithRatio();
            if(colour == Colour.None)
            {
                colour = colours[Random.Range(0, colours.Length)];
            }
            
            return  CreateItem(colour, parent);
        }

        private Colour GetRandomColourWithRatio()
        {
            int value = Random.Range(0, totalColourRatio);
            foreach (var temp in colourRatioDict.Keys)
            {
                if(value > temp.y) continue;
                if(value < temp.x) continue;
                return colourRatioDict[temp];
            }

            return Colour.None;
        }

        private void PrepareRatios()
        {
            colourRatioDict.Clear();
            totalColourRatio = 0;
            if(LevelManager.Instance.CurrentLevel.ColourRatios == null) return;
            for (var i = 0; i < LevelManager.Instance.CurrentLevel.ColourRatios.Length; i++)
            {
                var colourRatio = LevelManager.Instance.CurrentLevel.ColourRatios[i];
                var oldIndex = totalColourRatio;
                totalColourRatio += colourRatio.Ratio;
                var newIndex = totalColourRatio - 1;
                colourRatioDict.Add(new Vector2(oldIndex, newIndex), colourRatio.Colour);
            }
        }
        
        private Item CreateItemWithType(ItemBase itemBase, Type type, Colour colour = Colour.None)
        {
            var item = (Item)itemBase.gameObject.AddComponent(type);
            item.Prepare(itemBase, colour);

            return item;
        }
    }
}
