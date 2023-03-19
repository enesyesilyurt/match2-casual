using System;
using Casual.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Casual.Entities
{
    [Serializable]
    public class SpriteSet
    {
        [SerializeField] private Colour colour;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite propellerSprite;
        
        public Colour Colour => colour;
        public Sprite DefaultSprite => defaultSprite;
        public Sprite PropellerSprite => propellerSprite;
    }
}
