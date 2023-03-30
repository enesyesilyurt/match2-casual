using Casual.Utilities;
using UnityEngine;

namespace Casual.Controllers
{
    public class ItemBase : MonoBehaviour
    {
        [SerializeField] private FallAnimation fallAnimation;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public FallAnimation FallAnimation => fallAnimation;

        private const int BaseSortingOrder = 10;
        
        public void Prepare()
        {
            gameObject.SetActive(true);
            transform.SetParent(BoardController.Instance.ItemsParent);
        }

        public void PrepareFall()
        {
            
        }

        public void AddSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = BaseSortingOrder;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
        
        public void SetSortingOrder(int y)
        {
            spriteRenderer.sortingOrder = BaseSortingOrder + y;
        }

        public void IncreaseSortingOrder(int value)
        {
            spriteRenderer.sortingOrder += value;
        }
    }
}
