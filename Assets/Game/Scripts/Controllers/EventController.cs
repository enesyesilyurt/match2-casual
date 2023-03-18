using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
    [SerializeField] private UIMovableAnimationController[] activeElements;
    [SerializeField] private UIMovableAnimationController[] deactiveElements;
    [SerializeField] private bool isActive;

    private Button button;
    

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeSituation);
    }

    private void ChangeSituation()
    {
        isActive = !isActive;
        if (isActive) Activate();
        else Deactivate();
    }

    private void Activate()
    {
        foreach (var element in activeElements)
        {
            element.Activate();
        }

        foreach (var element in deactiveElements)
        {
            element.Deactivate();
        }
    }
    
    private void Deactivate()
    {
        foreach (var element in activeElements)
        {
            element.Deactivate();
        }
        
        foreach (var element in deactiveElements)
        {
            element.Activate();
        }
    }
}
