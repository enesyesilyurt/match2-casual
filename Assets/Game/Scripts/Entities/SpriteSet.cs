using System;
using Casual.Enums;
using UnityEngine;

namespace Casual.Entities
{
    [Serializable]
    public class SpriteSet
    {
        [SerializeField] private Colour colour;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite bombSprite;
        [SerializeField] private Sprite rocketSprite;
        [SerializeField] private Sprite specialSprite;
        
        public Colour Colour => colour;
        public Sprite DefaultSprite => defaultSprite;
        public Sprite BombSprite => bombSprite;
        public Sprite RocketSprite => rocketSprite;
        public Sprite SpecialSprite => specialSprite;
    }
}
