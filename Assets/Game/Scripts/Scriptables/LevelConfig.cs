using Casual.Entities;
using UnityEngine;

[CreateAssetMenu(menuName = "NewLevelConfig", fileName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private ColourRatio[] colourRatios;
    [SerializeField] private Row[] rows;

    public Vector2Int Size => size;
    public ColourRatio[] ColourRatios => colourRatios;
    public Row[] Rows => rows;
}

