using System;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct InputComponent : IComponentData
{
    /// <summary>
    /// Enabled returns if input should be refresh
    /// </summary>
    public bool Enabled;
    
    public float2 Move;
    
    public float3 Mouse;
    public float2 MouseWheel;
    
    public bool Shoot;
    public bool Reload;
    public bool Dash;
    public bool Interact;
    public bool Cancel;

    public GunType WeaponTypeDesired;
    
    public int Inventory;

    public void Reset()
    {
        //Set everything to not pressed
        Move = float2.zero;
        Mouse = float3.zero;
        Shoot = Reload = Dash = Interact = Cancel = false;
        Inventory = -1;
    }

    public void PartialReset()
    {
        Move = float2.zero;
        WeaponTypeDesired = GunType.NONE;
    }
    
    public void Update()
    {
        
    }
}
