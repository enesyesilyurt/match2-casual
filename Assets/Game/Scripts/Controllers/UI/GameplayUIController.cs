using System.Collections;
using System.Collections.Generic;
using Casual.Managers;
using TMPro;
using UnityEngine;

namespace Casual
{
    public class GameplayUIController : MonoBehaviour
    {
        [SerializeField] private FailPanelController failPanelController;
        [SerializeField] private InGameUIController inGameUIController;
        [SerializeField] private WinPanelController winPanelController;

        public void Initialize()
        {
            failPanelController.Initialize();
            inGameUIController.Initialize();
            winPanelController.Initialize();
            
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
        }

        public void ResetManager()
        {
            inGameUIController.ResetLevel();
        }
        
        private void Prepare()
        {
            gameObject.SetActive(true);
            failPanelController.Prepare();
            inGameUIController.Prepare();
            winPanelController.Prepare();
        }
        
        private void OnGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Home:
                    gameObject.SetActive(false);
                    break;
                case GameState.InGame:
                    Prepare();
                    break;
            }
        }
    }
}
