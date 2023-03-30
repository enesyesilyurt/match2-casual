using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Casual
{
    public class AreaController : MonoBehaviour
    {
        [SerializeField] private GameObject upgradeButtonContainer;
        [SerializeField] private Transform upgradeButtonParent;
        
        private AreaConfig areaConfig;

        private int currentPriorityIndex;
        
        public void Initialize(AreaConfig config)
        {
            areaConfig = config;
            ActivateUpgradeButtonsByPriority();
        }

        private void ActivateUpgradeButtonsByPriority()
        {
            for (int i = 0; i < areaConfig.UpgradeConfigs.Length; i++)
            {
                if (areaConfig.UpgradeConfigs[i].Priority == currentPriorityIndex)
                {
                    var button = SimplePool.Spawn(upgradeButtonContainer, areaConfig.UpgradeConfigs[i].ButtonPosition,
                        quaternion.identity);
                    button.SetActive(true);
                    button.GetComponent<UpgradeController>().Initialize(areaConfig.UpgradeConfigs[i], upgradeButtonParent);
                }
            }
        }
    }
}
