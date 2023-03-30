using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBase : ScriptableObject
{
    [SerializeField] protected int firstValue;
    [SerializeField] protected string key;
    [SerializeField] protected CollectibleType type;
        
    protected int value;

    public int Value => value;
    public CollectibleType Type => type;

    public event Action<int> ValueChanged;

    public void Initialize()
    {
        if (PlayerPrefs.GetInt(key + "IsFirst") == 0)
        {
            PlayerPrefs.SetInt(key + "IsFirst", 1);
            value = firstValue;
            PlayerPrefs.SetInt(key, value);
        }
        else
            value = PlayerPrefs.GetInt(key);
    }

    public virtual void ChangeValue(int amount)
    {
        PlayerPrefs.SetInt(key, value);
        ValueChanged?.Invoke(value);
    } 

    public virtual bool PurchaseValidation(int amount)
    {
        return value + amount >= 0;
    }
}
