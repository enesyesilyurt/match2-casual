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

        private Dictionary<Vector2, Colour> colourRatioDict = new();

        public void Setup()
        {
            PrepareRatios();
        }
        
        public Item CreateItem(Colour colour, Transform parent, ItemType itemType = ItemType.Default)
        {
            var itemBase = Instantiate(itemBasePrefab, Vector3.zero, Quaternion.identity, parent);

            Item item = null;
            switch (itemType)
            {
                case ItemType.Default:
                    item = CreateCubeItem(itemBase, colour);
                    break;
                case ItemType.Rocket:
                    break;
                case ItemType.DiscoBall:
                    break;
                case ItemType.BombItem:
                    item = CreateBombItem(itemBase);
                    break;
            }

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
            for (var i = 0; i < LevelManager.Instance.CurrentLevel.ColourRatios.Length; i++)
            {
                var colourRatio = LevelManager.Instance.CurrentLevel.ColourRatios[i];
                var oldIndex = totalColourRatio;
                totalColourRatio += colourRatio.Ratio;
                var newIndex = totalColourRatio - 1;
                colourRatioDict.Add(new Vector2(oldIndex, newIndex), colourRatio.Colour);
            }
        }
        
        private Item CreateCubeItem(ItemBase itemBase, Colour colour)
        {
            var cubeItem = itemBase.gameObject.AddComponent<CubeItem>();
            cubeItem.PrepareCubeItem(itemBase, colour);

            return cubeItem;
        }

        private Item CreateBombItem(ItemBase itemBase)
        {
            var bombItem = itemBase.gameObject.AddComponent<BombItem>();
            bombItem.PrepareBombItem(itemBase);

            return bombItem;
        }
    }
}
