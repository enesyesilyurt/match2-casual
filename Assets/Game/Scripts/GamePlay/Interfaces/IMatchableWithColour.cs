using System.Collections;
using System.Collections.Generic;
using Casual.Enums;
using UnityEngine;

namespace Casual
{
    public interface IMatchableWithColour
    {
        public Colour Colour { get; }
        int CheckMatchWithColour();
        void OnMatchCountChanged(int matchCount);
    }
}
