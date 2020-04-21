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

    public WeaponType WeaponTypeDesired;
    
    public int Inventory;

    public void Reset()
    {
        //Set everything to not pressed
        Move = float2.zero;
        Mouse = float3.zero;
        Shoot = Reload = Dash = Interact = Cancel = false;
        Inventory = -1;
    }

    private void PartialReset()
    {
        Move = float2.zero;
        WeaponTypeDesired = WeaponType.Pistol;
    }
    
    public void Update()
    {
        PartialReset();
        
        //Get input
        Mouse = Input.mousePosition;
        
        if (Input.GetKey(KeyCode.W))
            Move.y += 1;
        if (Input.GetKey(KeyCode.S))
            Move.y -= 1;
        
        if (Input.GetKey(KeyCode.A))
            Move.x -= 1;
        
        if (Input.GetKey(KeyCode.D))
            Move.x += 1;

        Interact = Input.GetKeyDown(KeyCode.E);

        Shoot = Input.GetMouseButton(0);

        Dash = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift);

        Reload = Input.GetKeyDown(KeyCode.R);

        Cancel = Input.GetKeyDown(KeyCode.Escape);
        
        //Weapon desired
        MouseWheel = Input.mouseScrollDelta;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            WeaponTypeDesired = WeaponType.Pistol;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            WeaponTypeDesired = WeaponType.Shotgun;
    }
}
