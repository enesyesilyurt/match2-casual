using Casual.Abstracts;
using Casual.Enums;
using Casual.Utilities;
using UnityEngine;

namespace Casual.Controllers.Items
{
    public class CubeItem : Item
    {
        public void PrepareCubeItem(ItemBase itemBase, Colour colour)
        {
            base.colour = colour;
            Prepare(itemBase, ImageLibrary.Instance.GetSprite(colour));
        }
    }
}
