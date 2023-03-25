using Casual.Abstracts;
using Casual.Enums;
using Casual.Managers;
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

        public override void ExecuteWithNeighbour()
        {
            
        }
        
        public override void ExecuteWithTapp()
        {
            base.ExecuteWithTapp();
            CreateParticle();
            foreach (var neighbor in CellController.GetNeighbours())
            {
                if(neighbor != null && neighbor.HasItem())
                    neighbor.Item.ExecuteWithNeighbour();
            }
            
            RemoveItem();
        }

        protected override void OnMatchCountChanged(int matchCount)
        {
            if (matchCount < GameManager.Instance.PropellerMatchCount)
            {
                spriteRenderer.sprite = ImageLibrary.Instance.GetSprite(colour);
                ItemType = ItemType.Cube;
            }
            else
            {
                spriteRenderer.sprite = ImageLibrary.Instance.GetSprite(colour, ItemType.MultipleCube);
                ItemType = ItemType.MultipleCube;
            }
        }

        private void CreateParticle()
        {
            var particle = SimplePool.Spawn(ParticleLibrary.Instance.GetParticle(colour).gameObject, transform.position,
                Quaternion.identity);
            particle.SetActive(true);
            particle.transform.SetParent(BoardController.Instance.ParticleParent);
        }
    }
}
