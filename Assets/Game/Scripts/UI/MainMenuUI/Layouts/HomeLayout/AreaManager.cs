using Casual.Managers;
using UnityEngine;

namespace Casual
{
    public class AreaManager : MonoBehaviour
    {
        [SerializeField] private AreaController areaController;
        [SerializeField] private AreaConfig[] areaConfigs;

        private int currentAreaIndex;

        public void Initialize()
        {
            var currentLevelIndex = LevelManager.Instance.CurrentLevelIndex;
            currentAreaIndex = GetAreaIndexByLevel(currentLevelIndex);
            var currentAreaConfig = areaConfigs[currentAreaIndex];
            
            areaController.Initialize(currentAreaConfig);
            
            LevelManager.Instance.CurrentLevelChanged += OnCurrentLevelChanged;
        }

        private void OnCurrentLevelChanged(int index)
        {
            var newLevelAreaIndex = GetAreaIndexByLevel(index);
            if(newLevelAreaIndex == currentAreaIndex) return;
            
            currentAreaIndex = newLevelAreaIndex;
            areaController.Initialize(areaConfigs[currentAreaIndex]);
        }

        private int GetAreaIndexByLevel(int index)
        {
            for (int i = currentAreaIndex; i < areaConfigs.Length; i++)
            {
                if(areaConfigs[i].StartLevel > index || areaConfigs[i].EndLevel < index) continue;
                return i;
            }

            return 0;
        }
    }
}
