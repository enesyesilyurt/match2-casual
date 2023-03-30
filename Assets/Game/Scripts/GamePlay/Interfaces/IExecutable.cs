using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Casual
{
    public interface IExecutable
    {
        void PrepareExecute();
        void Execute();
    }
}
