using System;
using System.Collections.Generic;
using Casual.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectibleManager : MonoSingleton<CollectibleManager>
{
    [SerializeField] private CollectibleBase[] collectibles;

    private Dictionary<CollectibleType, CollectibleBase> collectibleDict = new ();

    public void Setup()
    {
        foreach (var collectible in collectibles)
        {
            collectible.Initialize();
            collectibleDict.Add(collectible.Type, collectible);
        }
    }

    public CollectibleBase GetCollectible(CollectibleType type)
    {
        return collectibleDict[type];
    }
}
