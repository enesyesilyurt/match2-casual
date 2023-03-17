using Casual.Entities;
using UnityEngine;

[CreateAssetMenu(menuName = "NewLevelConfig", fileName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int ColumnCount;
    public int RowCount;
    public int maxMove;
    public ColourRatio[] ColourRatios;
    public SquareBlock[] Blocks;
    public Target[] Targets;
}

