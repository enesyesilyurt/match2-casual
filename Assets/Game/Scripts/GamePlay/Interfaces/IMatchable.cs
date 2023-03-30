using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Casual
{
    public interface IMatchableItem<T>
    {
        public T Value { get; }
        int CheckMatch();
    }
}
