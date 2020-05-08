using Enums;
using Static;
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
        InputComponent inputs = entityManager.GetComponentData<InputComponent>(GameVariables.Player.Entity);

        if (inputs.Enabled)
        {
            inputs.PartialReset();

            if (Input.GetKey(KeyCode.W))
            {
                inputs.Move.x += 1;
                inputs.Move.y += 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputs.Move.x -= 1;
                inputs.Move.y -= 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputs.Move.x -= 1;
                inputs.Move.y += 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputs.Move.x += 1;
                inputs.Move.y -= 1;
            }

            inputs.Move = math.normalizesafe(inputs.Move);
            entityManager.SetComponentData(GameVariables.Player.Entity, new DirectionData
            {
                Value = inputs.Move
            });
            
            inputs.Interact = Input.GetKeyDown(KeyCode.E);

            inputs.Shoot = Input.GetMouseButton(0);

            inputs.Dash = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift);

            inputs.Reload = Input.GetKeyDown(KeyCode.R);

            inputs.Cancel = Input.GetKeyDown(KeyCode.Escape);

            inputs.SwapWeapon1 = Input.GetKeyDown(KeyCode.Alpha1);
            
            inputs.SwapWeapon2 = Input.GetKeyDown(KeyCode.Alpha2);
            
            inputs.SwapWeapon3 = Input.GetKeyDown(KeyCode.Alpha3);

            //Weapon desired
            inputs.MouseWheel = Input.mouseScrollDelta;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                inputs.WeaponTypeDesired = WeaponType.Pistol;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                inputs.WeaponTypeDesired = WeaponType.Shotgun;
        }
        else
            inputs.Reset();

        
        inputs.Mouse = Input.mousePosition;

        entityManager.SetComponentData(GameVariables.Player.Entity, inputs);
        
        HoldMyBeer.ImBoosting(entityManager, Input.GetKey(KeyCode.KeypadEnter));
    }
}