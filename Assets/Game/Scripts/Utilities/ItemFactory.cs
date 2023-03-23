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
        
        public Item CreateItem(Colour colour, Transform parent, ItemType itemType = ItemType.Cube)
        {
            var itemBase = SimplePool.Spawn(itemBasePrefab.gameObject, Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<ItemBase>();
            itemBase.gameObject.SetActive(true);
            itemBase.transform.parent = parent;

            Item item = null;
            switch (itemType)
            {
                case ItemType.Cube:
                    item = CreateCubeItem(itemBase, colour);
                    break;
                case ItemType.Propeller:
                    item = CreatePropellerItem(itemBase);
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
            colourRatioDict.Clear();
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
        
        private Item CreateCubeItem(ItemBase itemBase, Colour colour)
        {
            var item = itemBase.gameObject.AddComponent<CubeItem>();
            item.PrepareCubeItem(itemBase, colour);

            return item;
        }

        private Item CreatePropellerItem(ItemBase itemBase)
        {
            var item = itemBase.gameObject.AddComponent<PropellerItem>();
            item.PreparePropellerItem(itemBase);

            return item;
        }
    }
}
