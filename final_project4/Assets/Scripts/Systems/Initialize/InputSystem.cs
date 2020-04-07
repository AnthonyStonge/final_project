using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class InputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref InputComponent input) =>
        {
            ResetInputs(ref input);
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                input.Move.y += 1;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                input.Move.y += -1;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                input.Move.x += -1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                input.Move.x += 1;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                input.Interact = true;
            }
            
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                input.Dash = true;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                input.Reload = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                input.Cancel = true;
            }
        }).Run();

        //TODO add number 1 to numberofweapons for cycling/changing weapon
        //TODO AND Mouse wheel to do the same thing
    }

    private static void ResetInputs(ref InputComponent ic)
    {
        ic.Cancel = false;
        ic.Dash = false;
        ic.Interact = false;
        ic.Reload = false;
        ic.Inventory = -1;
        ic.Mouse = float3.zero;
        ic.Move = float2.zero;
    }
}
