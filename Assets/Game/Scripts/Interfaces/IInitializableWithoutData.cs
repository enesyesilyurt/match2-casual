using System.Collections;
using System.Collections.Generic;
using Casual.Controllers;
using UnityEngine;

namespace Casual
{
    public interface IInitializableWithoutData
    {
        void InitializeWithoutData(ItemBase itemBase);
    }
}
