using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public struct FadeObject
{
    public Image Image;
    
    private float fadeValue;
    public float FadeValue
    {
        get => fadeValue;
        set => fadeValue = math.clamp(value, 0, 1);
    }

    private float speed;
    public float Speed
    {
        get => speed;
        set => speed = value * 0.01f;
    }
    
    public FadeType Type;
    public enum FadeType
    {
        FadeIn,
        FadeOut
    }
}
