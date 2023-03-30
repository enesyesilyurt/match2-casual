using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Casual
{
    public class WinPanelController : MonoBehaviour
    {
        public void Initialize()
        {
            TargetManager.Instance.TargetsCompleted += OpenWinPanel;
        }

        public void Prepare()
        {
            gameObject.SetActive(false);
        }
        
        private void OpenWinPanel()
        {
            gameObject.SetActive(true);
            UIManager.Instance.OpenPanel(gameObject);
        }
    }
}
