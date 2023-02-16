using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "NewLevel", fileName = "Level")]
public class LevelScriptable : ScriptableObject
{
    [BoxGroup("Level Design"), Space(30)]
    public Row Row1;
    [BoxGroup("Level Design"),Space(30)]
    public Row Row2;
    [BoxGroup("Level Design"),Space(30)]
    public Row Row3;
    [BoxGroup("Level Design"),Space(30)]
    public Row Row4;
    [BoxGroup("Level Design"),Space(30)]
    public Row Row5;
    [BoxGroup("Level Design"),Space(30)]
    public Row Row6;
    [BoxGroup("Level Design"),Space(30)]
    public Row Row7;
    [BoxGroup("Level Design"),Space(30)]
    public Row Row8;
    [BoxGroup("Level Design"),Space(30)]
    public Row Row9;
}
