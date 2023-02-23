using Casual.Controllers;
using Casual.Enums;
using Casual.Managers;
using Casual.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace Casual.Abstracts
{
    public abstract class Item : MonoBehaviour
    {
        private const int BaseSortingOrder = 10;

        public FallAnimation FallAnimation;
        
        private CellController cellController;
        private SpriteRenderer spriteRenderer;
        protected Colour colour;

        public Colour Colour => colour;
        
        public ItemType ItemType { get; private set; }
        
        public CellController CellController
        {
            get { return cellController; }
            set
            {
                if (cellController == value) return;

                var oldCell = cellController;
                cellController = value;

                if (oldCell != null && oldCell.Item == this)
                {
                    oldCell.Item = null;
                }

                if (value != null)
                {
                    value.Item = this;
                    gameObject.name = cellController.gameObject.name + " " + GetType().Name;
                }
            }
        }

        protected void Prepare(ItemBase itemBase, Sprite sprite)
        {
            AddSprite(sprite);
            FallAnimation = itemBase.FallAnimation;
            FallAnimation.Prepare(this);
        }

        public void SetSortingOrder(int y)
        {
            spriteRenderer.sortingOrder = BaseSortingOrder + y;
        }

        public void IncreaseSortingOrder(int value)
        {
            spriteRenderer.sortingOrder += value;
        }

        private void AddSprite(Sprite sprite)
        {
            spriteRenderer = new GameObject("Sprite_").AddComponent<SpriteRenderer>();
            spriteRenderer.transform.SetParent(transform);
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = BaseSortingOrder;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        public int CheckMatches()
        {
            var matchCount = BoardController.Instance.MatchFinder.FindMatches(cellController, colour).Count;
            SetSprite(matchCount);
            return matchCount - 1;
        }

        private void SetSprite(int matchCount)
        {
            if (matchCount < GameManager.Instance.BombMatchCount)
            {
                spriteRenderer.sprite = ImageLibrary.Instance.GetSprite(colour);
                ItemType = ItemType.Default;
            }
            else if (matchCount < GameManager.Instance.RocketMatchCount)
            {
                spriteRenderer.sprite = ImageLibrary.Instance.GetSprite(colour, ItemType.Bomb);
                ItemType = ItemType.Bomb;
            }
            else if (matchCount < GameManager.Instance.DiscoBallMatchCount)
            {
                spriteRenderer.sprite = ImageLibrary.Instance.GetSprite(colour, ItemType.Rocket);
                ItemType = ItemType.Rocket;
            }
            else
            {
                spriteRenderer.sprite = ImageLibrary.Instance.GetSprite(colour, ItemType.DiscoBall);
                ItemType = ItemType.DiscoBall;
            }
        }
        
        public void Fall()
        {
            FallAnimation.FallToTarget(CellController.GetFallTarget());
        }

        public void RemoveItem()
        {
            CellController.Item = null;
            CellController = null;
            Instantiate(ParticleLibrary.Instance.GetParticle(colour).gameObject, transform.position,
                quaternion.identity);

            Destroy(gameObject);
        }
    }
}
