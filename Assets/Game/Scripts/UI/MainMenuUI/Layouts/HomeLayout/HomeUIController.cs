using System;
using Casual.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI starText;
    [SerializeField] private TextMeshProUGUI areaUpgradeText;
    [SerializeField] private TextMeshProUGUI timedRewardText;
    [SerializeField] private Button playButton;
    
    [SerializeField] private HomeElementActivatorBase[] homeElements;
    [SerializeField] private ActivableUIElement[] movables;

    private int activeIndex;
    
    private Currency currency;
    private Health health;
    private Star star;
    private AreaUpgrade areaUpgrade;
    private TimedReward timedReward;

    public void Initialize()
    {
        // SetupActivableElements(); // TODO
        ReferanceAttachments();
        AddListenerCollectibles();
        SetupTexts();
            
        playButton.onClick.AddListener(()=> GameManager.Instance.ChangeGameState(GameState.InGame));
    }

    private void AddListenerCollectibles()
    {
        currency.ValueChanged += OnCurrencyChanged;
        health.ValueChanged += OnHealthChanged;
        star.ValueChanged += OnStarChanged;
        areaUpgrade.ValueChanged += OnAreaUpgradeChanged;
        timedReward.ValueChanged += OnTimedRewardChanged;
    }

    private void ReferanceAttachments()
    {
        currency = CollectibleManager.Instance.GetCollectible(CollectibleType.Currency) as Currency;
        health = CollectibleManager.Instance.GetCollectible(CollectibleType.Health) as Health;
        star = CollectibleManager.Instance.GetCollectible(CollectibleType.Star) as Star;
        areaUpgrade = CollectibleManager.Instance.GetCollectible(CollectibleType.Upgrade) as AreaUpgrade;
        timedReward = CollectibleManager.Instance.GetCollectible(CollectibleType.TimedReward) as TimedReward;
    }

    private void SetupTexts()
    {
        timedRewardText.text = timedReward.IsMaxed ? "FULL" : timedReward.Value + "/" + timedReward.MaxCount;
        healthText.text = health.IsMaxed ? "FULL" : health.Value.ToString();
        areaUpgradeText.text = areaUpgrade.IsMaxed ? "FULL" : areaUpgrade.Value + "/" + areaUpgrade.MaxCount;
        starText.text = star.IsMaxed ? "FULL" : star.Value.ToString();
        currencyText.text = currency.Value.ToString();
    }

    private void SetupActivableElements()
    {
        for (int i = 0; i < homeElements.Length; i++)
        {
            var elementIndex = i;
            
            if(homeElements[elementIndex].Button == null) continue;
            homeElements[elementIndex].Button.onClick.AddListener(()=>ActivateElements(elementIndex));
        }

        for (int i = 0; i < movables.Length; i++)
        {
            movables[i].Setup(false);
        }
        
        homeElements[0].SetupElements();
    }

    private void ActivateElements(int index)
    {
        if(homeElements[0].IsActive && index == 0) return;
        
        activeIndex = index == activeIndex ? 0 : index;
        
        for (int i = 0; i < homeElements.Length; i++)
        {
            homeElements[i].ChangeSituation(activeIndex == i);
        }
    }

    #region CallBacks
    
    private void OnCurrencyChanged(int newValue)
    {
        currencyText.text = newValue.ToString();
    }
    
    private void OnTimedRewardChanged(int newValue)
    {
        timedRewardText.text = timedReward.IsMaxed ? "FULL" : timedReward.Value + "/" + timedReward.MaxCount;
    }

    private void OnAreaUpgradeChanged(int newValue)
    {
        areaUpgradeText.text = areaUpgrade.IsMaxed ? "FULL" : areaUpgrade.Value + "/" + areaUpgrade.MaxCount;
    }

    private void OnStarChanged(int newValue)
    {
        starText.text = star.IsMaxed ? "FULL" : star.Value.ToString();
    }

    private void OnHealthChanged(int newValue)
    {
        healthText.text = health.IsMaxed ? "FULL" : health.Value.ToString();
    }
    
    #endregion
}
