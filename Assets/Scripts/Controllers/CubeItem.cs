using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeItem : Item
{
    public void PrepareCubeItem(ItemBase itemBase, MatchType matchType)
    {
        _matchType = matchType;
        SetItemType();
        Prepare(itemBase, GetSpritesForMatchType());
    }

    private void SetItemType()
    {
        switch (_matchType)
        {
            case MatchType.Green:
                ItemType = ItemType.GreenCube;
                break;
            case MatchType.Yellow:
                ItemType = ItemType.YellowCube;
                break;
            case MatchType.Blue:
                ItemType = ItemType.BlueCube;
                break;
            case MatchType.Red:
                ItemType = ItemType.RedCube;
                break;
        }
    }

    private Sprite GetSpritesForMatchType()
    {
        switch (_matchType)
        {
            case MatchType.Green:
                return ImageLibrary.Instance.GreenCubeSprite;
            case MatchType.Yellow:
                return ImageLibrary.Instance.YellowCubeSprite;
            case MatchType.Blue:
                return ImageLibrary.Instance.BlueCubeSprite;
            case MatchType.Red:
                return ImageLibrary.Instance.RedCubeSprite;
        }

        return null;
    }

    public override void TryExecute()
    {
        // ServiceProvider.GetParticleManager.PlayCubeParticle(this);
            
        base.TryExecute();
    }
}
