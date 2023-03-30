using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HomeElementActivatorBase
{
    public Button Button;
    [SerializeField] private ActivableUIElement[] activeElements;
    [SerializeField] private ActivableUIElement[] deactiveElements;
        
    private bool isActive;
    public bool IsActive => isActive;

    public void ChangeSituation(bool newSituation)
    {
        if(isActive == newSituation) return;
        isActive = newSituation;
        if(!isActive) return;
            
        for (int i = 0; i < activeElements.Length; i++) 
            activeElements[i].ChangeSituation(isActive);
            
        for (int i = 0; i < deactiveElements.Length; i++) 
            deactiveElements[i].ChangeSituation(!isActive);
    }

    public void SetupElements()
    {
        isActive = true;
        foreach (var element in activeElements) element.Setup(true);
    }
}
