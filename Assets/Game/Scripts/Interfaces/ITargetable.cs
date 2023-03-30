using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Casual
{
    public interface ITargetable<out T>
    {
        T Value { get; }
    }
}