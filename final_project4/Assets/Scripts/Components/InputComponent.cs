using System;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct InputComponent : IComponentData
{
    public float3 Mouse;
    public float2 MouseWheel;
    public float2 Move;
    
    public int Inventory;
   
    public bool Shoot;
    public bool Reload;
    public bool Dash;
    public bool Interact;
    public bool Cancel;
    
    /// <summary>
    /// Enabled returns if input should be refresh
    /// </summary>
    public bool Enabled;

    public WeaponType WeaponTypeDesired;

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
        WeaponTypeDesired = GameVariables.Player.CurrentWeaponHeld;
    }
}
