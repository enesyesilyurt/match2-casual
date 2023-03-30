using System.Collections;
using System.Collections.Generic;
using Casual.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Casual
{
    public class FailPanelController : MonoBehaviour
    {
        [SerializeField, Header("Buttons")] private Button ContinueShadowButton;
        [SerializeField] private Button TryAgainShadowButton;
        [SerializeField] private Button TryAgainExitButton;
        [SerializeField] private Button ContinueExitButton;
        
        [SerializeField, Header("Panels")] private GameObject tryAgainPanel;
        [SerializeField] private GameObject continuePanel;

        public void Initialize()
        {
            TargetManager.Instance.LevelFailed += OpenFailPanel;
            
            ContinueExitButton.onClick.AddListener(OpenTryAgainPanel);
            ContinueShadowButton.onClick.AddListener(OpenTryAgainPanel);
            TryAgainExitButton.onClick.AddListener(()=>GameManager.Instance.ChangeGameState(GameState.Home));
            TryAgainShadowButton.onClick.AddListener(()=>GameManager.Instance.ChangeGameState(GameState.Home));
        }
        
        public void Prepare()
        {
            gameObject.SetActive(false);
            tryAgainPanel.SetActive(false);
            continuePanel.SetActive(false);
        }

        private void OpenFailPanel()
        {
            gameObject.SetActive(true);
            tryAgainPanel.SetActive(false);
            continuePanel.SetActive(true);
            UIManager.Instance.OpenPanel(gameObject);
        }

        private void OpenTryAgainPanel()
        {
            continuePanel.SetActive(false);
            tryAgainPanel.SetActive(true);
        }
    }
}
