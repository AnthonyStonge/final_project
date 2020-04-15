using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisableAutoCreation]
public class InputSystem : SystemBase
{
    private EntityManager entityManager;
    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        InputComponent input = entityManager.GetComponentData<InputComponent>(GameVariables.PlayerVars.Entity);

        input.Mouse = Input.mousePosition;
        
        if (Input.GetKeyDown(KeyCode.W))
            input.Move.y = 1;
        if (Input.GetKeyUp(KeyCode.W))
            input.Move.y = 0;
        
        if (Input.GetKeyDown(KeyCode.S))
            input.Move.y = -1;
        if (Input.GetKeyUp(KeyCode.S))
            input.Move.y = 0;
        
        if (Input.GetKeyDown(KeyCode.A))
            input.Move.x = -1;
        if (Input.GetKeyUp(KeyCode.A))
            input.Move.x = 0;
        
        else if (Input.GetKeyDown(KeyCode.D))
            input.Move.x = 1;
        else if (Input.GetKeyUp(KeyCode.D))
            input.Move.x = 0;
        if (Input.GetKeyDown(KeyCode.E))
            input.Interact = true;
        
        if (Input.GetMouseButton(0))
            input.Shoot = true;
        //
        // if (Input.GetMouseButtonUp(0))
        //     input.Shoot = false;
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift))
            input.Dash = true;
        
        if (Input.GetKeyDown(KeyCode.R))
            input.Reload = true;
        
        if (Input.GetKeyDown(KeyCode.Escape))
            input.Cancel = true;
        
        
        entityManager.SetComponentData(GameVariables.PlayerVars.Entity, input);

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
