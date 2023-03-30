using System;
using System.Collections.Generic;
using Casual.Controllers.Items;
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
        [SerializeField] private Sprite balloonSprite;
        [SerializeField] private Sprite boxSprite;
        [SerializeField] private Sprite pumpkinSprite;
        [SerializeField] private Sprite bushSprite;
        [SerializeField] private Sprite bubbleSprite;

        [Header("EditorTextures")] 
        public Texture BlueCube;
        public Texture GreenCube;
        public Texture PinkCube;
        public Texture PurpleCube;
        public Texture RedCube;
        public Texture YellowCube;
        public Texture Balloon;
        public Texture Pumpkin;
        public Texture Box;
        public Texture Bubble;
        public Texture Bush;
        
        private Dictionary<Colour, SpriteSet> colourSetsDict = new();

        public void Setup()
        {
            for (int i = 0; i < colourSets.Length; i++)
            {
                colourSetsDict.Add(colourSets[i].Colour, colourSets[i]);
            }
        }

        public Sprite GetSprite(string itemType, Colour colour = Colour.Empty, bool isDefault = true)
        {
            switch (itemType)
            {
                case nameof(CubeItem):
                    return isDefault ? colourSetsDict[colour].DefaultSprite : colourSetsDict[colour].PropellerSprite;
                case nameof(BalloonItem):
                    return balloonSprite;
                case nameof(PropellerItem):
                    return propellerSprite;
                case nameof(BoxItem):
                    return boxSprite;
                case nameof(PumpkinItem):
                    return pumpkinSprite;
                case nameof(BubbleObstacle):
                    return bubbleSprite;
                case nameof(BushObstacle):
                    return bushSprite;
                default:
                    return colourSetsDict[colour].DefaultSprite;
            }
        }
    }
}
