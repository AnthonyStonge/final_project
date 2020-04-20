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
            inputs.Update();
        else
            inputs.Reset();
        
        entityManager.SetComponentData(GameVariables.PlayerVars.Entity, inputs);
    }
}