using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

namespace Casual
{
    public class AreaController : MonoBehaviour
    {
        [SerializeField] private GameObject upgradeButtonContainer;
        [SerializeField] private Transform upgradeButtonParent;

        private List<UpgradeController> activeUpgradeButtons = new ();
        private AreaConfig areaConfig;
        private int currentPriorityIndex;
        
        public void Initialize(AreaConfig config)
        {
            areaConfig = config;
            ActivateUpgradeButtonsByPriority();
        }

        [Button]
        public void IncreaseUpgradeLevel()
        {
            currentPriorityIndex++;
            ActivateUpgradeButtonsByPriority();
        }

        private void ActivateUpgradeButtonsByPriority()
        {
            foreach (var button in activeUpgradeButtons)
            {
                button.Despawn();
            }
            activeUpgradeButtons.Clear();
            
            for (int i = 0; i < areaConfig.UpgradeConfigs.Length; i++)
            {
                if (areaConfig.UpgradeConfigs[i].Priority == currentPriorityIndex)
                {
                    var buttonGO = SimplePool.Spawn(upgradeButtonContainer, areaConfig.UpgradeConfigs[i].ButtonPosition,
                        quaternion.identity);
                    var upgradeController = buttonGO.GetComponent<UpgradeController>();
                    activeUpgradeButtons.Add(upgradeController);
                    upgradeController.Initialize(areaConfig.UpgradeConfigs[i], upgradeButtonParent);
                }
            }
        }
    }
}
