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
            base.ItemType = ItemType.Cube;
            Prepare(itemBase, ImageLibrary.Instance.GetSprite(colour));
        }

        public override void TryExecute()
        {
            CreateParticle();
            base.TryExecute();
        }

        private void CreateParticle()
        {
            SimplePool.Spawn(ParticleLibrary.Instance.GetParticle(colour).gameObject, transform.position,
                Quaternion.identity);
        }
    }
}
