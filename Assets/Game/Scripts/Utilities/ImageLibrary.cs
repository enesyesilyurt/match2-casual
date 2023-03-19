using System.Collections.Generic;
using Casual.Entities;
using Casual.Enums;
using UnityEngine;

namespace Casual.Utilities
{
    public class ImageLibrary : MonoSingleton<ImageLibrary>
    {
        [SerializeField] private SpriteSet[] colourSets;

        [Header("Specials")]
        [SerializeField] private Sprite propellerSprite;

        [Header("EditorTextures")] 
        public Texture BlueCube;
        public Texture GreenCube;
        public Texture PinkCube;
        public Texture PurpleCube;
        public Texture RedCube;
        public Texture YellowCube;
        public Texture Bomb;
        public Texture Propeller;
        public Texture Rocket;
        
        private Dictionary<Colour, SpriteSet> colourSetsDict = new();

        public void Setup()
        {
            for (int i = 0; i < colourSets.Length; i++)
            {
                colourSetsDict.Add(colourSets[i].Colour, colourSets[i]);
            }
        }

        public Sprite GetSprite(Colour colour, ItemType itemType = ItemType.Cube)
        {
            switch (itemType)
            {
                case ItemType.Propeller:
                    return colourSetsDict[colour].PropellerSprite;
                default:
                    return colourSetsDict[colour].DefaultSprite;
            }
        }

        public Sprite GetSpecialSprite(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.PropellerItem:
                    return propellerSprite;
                default:
                    return propellerSprite;
            }
        }
    }
}
