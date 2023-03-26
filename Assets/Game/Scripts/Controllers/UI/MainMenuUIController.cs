using System.Collections;
using System.Collections.Generic;
using Casual.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Casual
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private BottomSelectorController bottomSelectorController;
        [SerializeField] private MainLayoutsUIController mainLayoutsUIController;
        
        public void Initialize()
        {
            bottomSelectorController.Initialize();
            mainLayoutsUIController.Initialize();
            
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Home:
                    gameObject.SetActive(true);
                    break;
                case GameState.InGame:
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}
