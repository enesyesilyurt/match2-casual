using System;
using Casual.Enums;
using UnityEngine;

namespace Casual.Entities
{
    [Serializable]
    public class ColourRatio
    {
        [SerializeField] private Colour colour;
        [SerializeField] private int ratio;

        public Colour Colour => colour;
        public int Ratio => ratio;
    }
}
