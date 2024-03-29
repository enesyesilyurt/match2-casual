using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Casual
{
    [CreateAssetMenu(menuName = "UI/UpgradeConfig", fileName = "UpgradeConfig")]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private string upgradeName;
        [SerializeField] private int priority;
        [SerializeField] private int starCount;
        [SerializeField] private Vector3 buttonPosition;
        
        public int Priority => priority;
        public int StarCount => starCount;
        public string Name => upgradeName;
        public Vector3 ButtonPosition => buttonPosition;
    }
}
