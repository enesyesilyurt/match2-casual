using Casual.Entities;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "NewLevelConfig", fileName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int GridHeight;
    public int GridWidth;
    public int maxMove;
    public ColourRatio[] ColourRatios;
    public SquareBlock[] Blocks;
    public Target[] Targets;
}

