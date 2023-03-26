using System.Collections;
using System.Collections.Generic;
using Casual.Controllers;
using UnityEngine;

namespace Casual
{
    public abstract class Obstacle : MonoBehaviour
    {
        private const int BaseSortingOrder = 10;
        private CellController cellController;
        private SpriteRenderer spriteRenderer;
        
        public CellController CellController
        {
            get => cellController;
            set
            {
                cellController = value;
                gameObject.name = cellController.gameObject.name + " " + GetType().Name;
            }
        }
        
        public virtual void Prepare(CellController cell)
        {
            
        }

        public virtual void OnItemExecuted()
        {
            
        }
        
        public virtual void OnNeighbourExecute() { }
        
        protected void AddSprite(Sprite sprite)
        {
            var tempRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer = tempRenderer == null
                ? gameObject.AddComponent<SpriteRenderer>()
                : spriteRenderer = tempRenderer;

            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = BaseSortingOrder;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        
        public void SetSortingOrder(int y)
        {
            spriteRenderer.sortingOrder = BaseSortingOrder + y;
        }
    }
}
