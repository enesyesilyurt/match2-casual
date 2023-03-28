using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Casual
{
    public interface IExecutableWithNeighbor : IExecutable
    {
        void ExecuteWithNeighbor();
    }
}
