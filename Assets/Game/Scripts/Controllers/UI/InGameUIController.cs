using System.Collections;
using System.Collections.Generic;
using Casual.Managers;
using TMPro;
using UnityEngine;

namespace Casual
{
    public class InGameUIController : MonoBehaviour
    {
        [SerializeField] private FailPanelController failPanelController;
        [SerializeField] private TargetController targetPrefab;
        [SerializeField] private TextMeshProUGUI moveCountText;
        [SerializeField] private Transform targetParent;
        
        private List<TargetController> targetList = new ();

        public void Initialize()
        {
            failPanelController.Initialize();
        }
        
        public void SetupPanel()
        {
            failPanelController.Prepare();
            targetList.Clear();
        
            var targets = LevelManager.Instance.CurrentLevel.Targets;
            for (int i = 0; i < targets.Length; i++)
            {
                var targetObject = Instantiate(targetPrefab, targetParent);
                targetList.Add(targetObject);
                targetObject.Setup(targets[i]);
            }
        
            moveCountText.text = LevelManager.Instance.CurrentLevel.maxMove.ToString();

            TargetManager.Instance.MoveCountChanged += DecreaseMoveCount;
        }

        public void ResetManager()
        {
            foreach (var target in targetList)
            {
                if(target != null)
                    Destroy(target.gameObject);
            }
        }

        private void DecreaseMoveCount(int count)
        {
            moveCountText.text = count.ToString();
        }
    }
}
