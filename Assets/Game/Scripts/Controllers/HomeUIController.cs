using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HomeUIController : MonoBehaviour
{
    [Serializable]
    private class HomeElementsActivite
    {
        public Button Button;
        
        [SerializeField] private UIMovableAnimationController[] activeElements;
        [SerializeField] private UIMovableAnimationController[] deactiveElements;
        
        private bool isActive;
        public bool IsActive => isActive;

        public void ChangeSituation(bool newSituation)
        {
            if(isActive == newSituation) return;
            isActive = newSituation;
            if(!isActive) return;
            foreach (var element in activeElements) element.ChangeSituation(isActive);
            foreach (var element in deactiveElements) element.ChangeSituation(!isActive);
        }

        public void SetupElements()
        {
            isActive = true;
            foreach (var element in activeElements) element.Setup(true);
        }
    }

    [SerializeField] private HomeElementsActivite[] homeElements;
    [SerializeField] private UIMovableAnimationController[] movables;

    private int activeIndex;

    private void Start()
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

    #region Button Methods

    #endregion
}
