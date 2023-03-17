using Casual.Utilities;
using UnityEngine;

namespace Casual.Managers
{
    public class ScreenManager : MonoSingleton<ScreenManager>
    {
        [SerializeField] private Camera cam;
        
        public void Setup()
        {
            PrepareCamera();
        }
        
        private void PrepareCamera()
        {
            cam.orthographicSize = (LevelManager.Instance.CurrentLevel.RowCount / cam.aspect) / 2 + GameManager.Instance.OffsetX;

            cam.transform.position -= Vector3.left * (LevelManager.Instance.CurrentLevel.RowCount * GameManager.Instance.OffsetX / 2f - .5f) +
                                  Vector3.down * LevelManager.Instance.CurrentLevel.ColoumnCount * GameManager.Instance.OffsetY / 2f;
        }
    }
}
