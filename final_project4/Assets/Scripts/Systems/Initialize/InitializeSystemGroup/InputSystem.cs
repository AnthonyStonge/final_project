using Enums;
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
        InputComponent inputs = entityManager.GetComponentData<InputComponent>(GameVariables.PlayerVars.Entity);

        if (inputs.Enabled)
        {
            inputs.PartialReset();
        
            //Get input
            inputs.Mouse = Input.mousePosition;
        
            if (Input.GetKey(KeyCode.W))
                inputs.Move.y += 1;
            if (Input.GetKey(KeyCode.S))
                inputs.Move.y -= 1;
        
            if (Input.GetKey(KeyCode.A))
                inputs.Move.x -= 1;
        
            if (Input.GetKey(KeyCode.D))
                inputs.Move.x += 1;

            inputs.Interact = Input.GetKeyDown(KeyCode.E);

            inputs.Shoot = Input.GetMouseButton(0);

            inputs.Dash = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift);

            inputs.Reload = Input.GetKeyDown(KeyCode.R);

            inputs.Cancel = Input.GetKeyDown(KeyCode.Escape);
        
            //Weapon desired
            inputs.MouseWheel = Input.mouseScrollDelta;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                inputs.WeaponTypeDesired = GunType.PISTOL;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                inputs.WeaponTypeDesired = GunType.SHOTGUN;
        }
        else
            inputs.Reset();
        
        entityManager.SetComponentData(GameVariables.PlayerVars.Entity, inputs);
    }
}