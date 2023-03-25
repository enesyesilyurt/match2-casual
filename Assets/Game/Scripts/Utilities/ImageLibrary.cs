using System.Collections.Generic;
using Casual.Entities;
using Casual.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Casual.Utilities
{
    public class ImageLibrary : MonoSingleton<ImageLibrary>
    {
        [SerializeField] private SpriteSet[] colourSets;

        [Header("Specials")]
        [SerializeField] private Sprite propellerSprite;
        [SerializeField] private Sprite balloonSprite;
        [SerializeField] private Sprite boxSprite;

        [Header("EditorTextures")] 
        public Texture BlueCube;
        public Texture GreenCube;
        public Texture PinkCube;
        public Texture PurpleCube;
        public Texture RedCube;
        public Texture YellowCube;
        public Texture Balloon;
        public Texture Propeller;
        
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
                case ItemType.MultipleCube:
                    return colourSetsDict[colour].PropellerSprite;
                case ItemType.Cube:
                    return colourSetsDict[colour].DefaultSprite;
                case ItemType.Balloon:
                    return balloonSprite;
                case ItemType.Propeller:
                    return propellerSprite;
                case ItemType.Box:
                    return boxSprite;
                default:
                    return colourSetsDict[colour].DefaultSprite;
            }
        }
    }
}
