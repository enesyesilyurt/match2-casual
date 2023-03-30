using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivableUIElement : MonoBehaviour
{
    [SerializeField] protected float inDelay;
    [SerializeField] protected float outDelay;
    
    protected bool isActive;
    
    public abstract void Setup(bool isActive);
    public abstract void ChangeSituation(bool newSituation);
}
