using System.Collections.Generic;
using Casual.Entities;
using NaughtyAttributes;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "NewLevelConfig", fileName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int StarCount;
    public int CoinCount;
    public int GridHeight;
    public int GridWidth;
    public int MaxMove;
    public List<ItemRatio> ItemRatios;
    public ItemData[] ItemDatas;
    public List<Target> Targets;
}

