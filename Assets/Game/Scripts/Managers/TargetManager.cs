using System.Collections;
using System.Collections.Generic;
using Casual.Managers;
using Casual.Utilities;
using UnityEngine;

public class TargetManager : MonoSingleton<TargetManager>
{
    private Target[] targets;
    
    public void Setup()
    {
        targets = LevelManager.Instance.CurrentLevel.Targets;
    }
}
