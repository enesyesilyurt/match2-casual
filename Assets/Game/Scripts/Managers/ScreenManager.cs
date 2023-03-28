using Casual.Utilities;
using UnityEngine;

namespace Casual.Managers
{
    public class ScreenManager : MonoSingleton<ScreenManager>
    {
        [SerializeField] private Camera cam;
        
        public void Setup()
        {
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newState)
        {
            if(newState == GameState.InGame)
                PrepareCamera();
        }

        private void PrepareCamera()
        {
            cam.transform.position = Vector3.right * (LevelManager.Instance.CurrentLevel.GridWidth * GameManager.Instance.OffsetX / 2f - .5f) +
                                     Vector3.up * (LevelManager.Instance.CurrentLevel.GridHeight * GameManager.Instance.OffsetY / 2f) + Vector3.back;
        }
    }
}
