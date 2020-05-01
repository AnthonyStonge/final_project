using System;
using Unity.Mathematics;
using UnityEngine;
[Serializable]
public struct Range
{
    [Tooltip("Minimum Value in Range")]
    public float Min;
    
    [Tooltip("Maximum Value in Range")]
    public float Max;
    
    [SerializeField]
    [Tooltip("Current Value in Range")]
    private float value;

    public Range(float min, float max, float value)
    {
        Min = min;
        Max = max;
        this.value = value;
        Value = this.value;
    }
    public float Value
    {
        get => value;
        set => math.clamp(value, Min, Max);
    }
    
    
}
