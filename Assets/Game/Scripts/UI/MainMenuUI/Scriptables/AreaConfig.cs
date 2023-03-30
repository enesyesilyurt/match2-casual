using UnityEngine;
using UnityEngine.UI;

namespace Casual
{
    [CreateAssetMenu(menuName = "UI/AreaConfig", fileName = "AreaConfig")]
    public class AreaConfig : ScriptableObject
    {
        [SerializeField] private string areaName;
        [SerializeField] private int startLevel;
        [SerializeField] private int endLevel;
        [SerializeField] private Image menuBackGround;

        [SerializeField] private UpgradeConfig[] upgradeConfigs;
        
        public string Name => areaName;
        public int StartLevel => startLevel;
        public int EndLevel => endLevel;
        public Image MenuBackGround => menuBackGround;
        public UpgradeConfig[] UpgradeConfigs => upgradeConfigs;
    }
}
