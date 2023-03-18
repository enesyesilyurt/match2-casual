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
            cam.orthographicSize = (9 / cam.aspect) / 2 + GameManager.Instance.OffsetX;

            cam.transform.position = Vector3.right * (9 * GameManager.Instance.OffsetX / 2f - .5f) +
                                     Vector3.up * 9 * GameManager.Instance.OffsetY / 2f + Vector3.back;
        }
    }
}
