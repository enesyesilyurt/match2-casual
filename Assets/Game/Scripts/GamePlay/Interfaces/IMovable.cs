using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Casual
{
    public interface IMovable
    {
        bool CanMove { get; }
        void Move();
    }
}
