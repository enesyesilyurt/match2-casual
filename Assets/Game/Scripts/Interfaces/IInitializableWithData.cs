using System.Collections;
using System.Collections.Generic;
using Casual.Controllers;
using UnityEngine;

namespace Casual
{
    public interface IInitializableWithData
    {
        void InitializeWithData(ItemData itemData, ItemBase itemBase);
    }
}
