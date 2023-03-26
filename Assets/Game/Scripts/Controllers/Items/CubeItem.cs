using Casual.Abstracts;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using UnityEngine;

namespace Casual.Controllers.Items
{
    public class CubeItem : Item
    {
        public override void Prepare(ItemBase itemBase, Colour asd)
        {
            base.colour = asd;
            base.ItemType = ItemType.Cube;
            base.Prepare(itemBase, colour);
            AddSprite(ImageLibrary.Instance.GetSprite(asd));
        }

        public override void OnNeighbourExecute()
        {
            
        }
        
        public override void ExecuteWithTapp()
        {
            if(!CellController.IsItemCanExecute) return;
            base.ExecuteWithTapp();
            CreateParticle();
            foreach (var neighbor in CellController.GetNeighbours())
            {
                if(neighbor != null && neighbor.HasItem())
                    neighbor.Item.OnNeighbourExecute();
                
                if(neighbor != null && neighbor.HasObstacle())
                    neighbor.Obstacle.OnNeighbourExecute();
            }
            
            RemoveItem();
        }

        public override void ExecuteWithSpecial()
        {
            if(!CellController.IsItemCanExecute) return;
            base.ExecuteWithSpecial();
            CreateParticle();
            foreach (var neighbor in CellController.GetNeighbours())
            {
                if(neighbor != null && neighbor.HasItem())
                    neighbor.Item.OnNeighbourExecute();

                if(neighbor != null && neighbor.HasObstacle())
                    neighbor.Obstacle.OnNeighbourExecute();
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
