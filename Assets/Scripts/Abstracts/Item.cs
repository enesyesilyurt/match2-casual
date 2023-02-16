using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Item : MonoBehaviour
{
    private const int BaseSortingOrder = 10;

    public SpriteRenderer SpriteRenderer;
    public FallAnimation FallAnimation;
    public ItemType ItemType;

    private int _childSpriteOrder;
    private CellController _cellController;
    protected MatchType _matchType;

    public MatchType MatchType => _matchType;
    
    public CellController CellController
    {
        get { return _cellController; }
        set
        {
            if (_cellController == value) return;

            var oldCell = _cellController;
            _cellController = value;

            if (oldCell != null && oldCell.Item == this)
            {
                oldCell.Item = null;
            }

            if (value != null)
            {
                value.Item = this;
                gameObject.name = _cellController.gameObject.name + " " + GetType().Name;
            }
        }
    }
    
    public void Prepare(ItemBase itemBase, Sprite sprite)
    {
        SpriteRenderer = AddSprite(sprite);
        FallAnimation = itemBase.FallAnimation;
        FallAnimation.Item = this;
    }
    
    public SpriteRenderer AddSprite(Sprite sprite)
    {
        var spriteRenderer = new GameObject("Sprite_" + _childSpriteOrder).AddComponent<SpriteRenderer>();
        spriteRenderer.transform.SetParent(transform);
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingLayerID = SortingLayer.NameToID("Item");
        spriteRenderer.sortingOrder = BaseSortingOrder + _childSpriteOrder++;

        return spriteRenderer;
    }
    
    public virtual void Fall()
    {
        FallAnimation.FallTo(CellController.GetFallTarget());				
    }
           
    public virtual void TryExecute()
    {
        RemoveItem();
    }

    public void RemoveItem()
    {
        CellController.Item = null;
        CellController = null;

        Destroy(gameObject);
    }

    public override string ToString()
    {
        return gameObject.name;
    }
}
