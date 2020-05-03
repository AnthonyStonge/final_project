using System;
using Unity.Mathematics;
using UnityEngine;
[Serializable]
public struct Range
{
    // [Tooltip("Minimum Value in Range")]
    public float Min;
    
    // [Tooltip("Maximum Value in Range")]
    public float Max;
    
    [SerializeField]
    // [Tooltip("Current Value in Range")]
    private float hiddenValue;

    public Range(float min, float max, float value)
    {
        Min = min;
        Max = max;
        this.hiddenValue = value;
        Value = this.hiddenValue;
    }
    public float Value
    {
        get => hiddenValue;
        set => hiddenValue = math.clamp(value, Min, Max);
       

    }
    
    
}
