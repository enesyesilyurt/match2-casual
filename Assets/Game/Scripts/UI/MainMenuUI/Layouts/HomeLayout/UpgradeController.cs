using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Casual
{
    public class UpgradeController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI starCountText;
        [SerializeField] private UpgradeConfig upgradeConfig;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float inDelay;
        [SerializeField] private float outDelay;
        
        private bool isActive;
        
        public void Initialize(UpgradeConfig config, Transform parent)
        {
            upgradeConfig = config;
            transform.SetParent(parent);
            button.onClick.AddListener(OnButtonClicked);
            gameObject.SetActive(true);
            Setup();
            HomeUIController.BuildButtonClicked += OnBuildButtonClicked;
        }

        private void OnBuildButtonClicked()
        {
            ChangeSituation();
        }

        private void OnButtonClicked()
        {
            Debug.Log(upgradeConfig.Name);
        }

        private void Setup()
        {
            transform.position = upgradeConfig.ButtonPosition;
            starCountText.text = upgradeConfig.StarCount.ToString();
            nameText.text = upgradeConfig.Name;
            canvasGroup.alpha = HomeUIController.BuildsActive ? 1 : 0;
            gameObject.SetActive(HomeUIController.BuildsActive);
        }

        public void Despawn()
        {
            button.onClick.RemoveListener(OnButtonClicked);
            HomeUIController.BuildButtonClicked -= OnBuildButtonClicked;
            SimplePool.Despawn(gameObject);
        }

        private void ChangeSituation()
        {
            isActive = HomeUIController.BuildsActive;
            gameObject.SetActive(true);
            transform.localScale = Vector3.one * (isActive ? .7f : 1);
            transform.DOScale(isActive ? 1 : .7f, .3f).SetEase(isActive ? Ease.OutBack : Ease.InBack)
                .SetDelay(isActive ? inDelay : outDelay);
        
            canvasGroup.DOFade(isActive ? 1 : 0, .3f).SetDelay(isActive ? inDelay : outDelay)
                .OnComplete(()=> gameObject.SetActive(isActive));
        }
    }
}
