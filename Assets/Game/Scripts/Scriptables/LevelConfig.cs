using System.Collections.Generic;
using Casual.Entities;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "NewLevelConfig", fileName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int ColoumnCount;
    public int RowCount;
    public ColourRatio[] ColourRatios;
    public SquareBlock[] Blocks;
}

