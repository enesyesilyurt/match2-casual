using UnityEngine;
using UnityEngine.UI;

namespace Casual
{
    public class UpgradeController : MonoBehaviour
    {
        [SerializeField] private Button button;
        private UpgradeConfig upgradeConfig;
        
        public void Initialize(UpgradeConfig config, Transform parent)
        {
            upgradeConfig = config;
            transform.SetParent(parent);
            button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Debug.Log(upgradeConfig.Name);
        }
    }
}
