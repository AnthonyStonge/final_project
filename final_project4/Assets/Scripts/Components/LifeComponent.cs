using System;
using Enums;
using Unity.Entities;
using Unity.Properties;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct LifeComponent : IComponentData
{
    public Range Life;
    public bool IsInvincible { get; private set; }
    public InvincibilityType Invincibility { get; private set; }

    public void SetInvincibility(InvincibilityType type)
    {
        IsInvincible = true;
        Invincibility = type;
    }

    public void StopInvincibility()
    {
        IsInvincible = false;
    }
    
    public bool DecrementLife()
    {
        if (!IsInvincible)
        {
            Life.Value--;
            SetInvincibility(InvincibilityType.Hit);
            return true;
        }

        return false;
    }
    
    public void Reset()
    {
        Life.Value = Life.Max;
        Debug.Log("Max : " + Life.Max);
    }
}
