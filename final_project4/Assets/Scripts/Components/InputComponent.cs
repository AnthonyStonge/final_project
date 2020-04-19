using System;
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
    public bool Shoot;
    public bool Reload;
    public bool Dash;
    public bool Interact;
    public int Inventory;
    public bool Cancel;
    public float3 Mouse;

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
    }
}
