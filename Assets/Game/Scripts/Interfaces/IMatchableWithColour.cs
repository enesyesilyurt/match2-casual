using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Casual
{
    public interface IMatchableWithColour
    {
        int CheckMatchWithColour();
        void OnMatchCountChanged(int matchCount);
    }
}
