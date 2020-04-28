using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static GameVariables;

[DisableAutoCreation]
[AlwaysUpdateSystem]
[UpdateAfter(typeof(StateMovingSystem))]
public class StateAttackingSystem : SystemBase
{
    private EntityManager entityManager;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        //Player state
        Entity player = GameVariables.Player.Entity;
        InputComponent playerInputs = entityManager.GetComponentData<InputComponent>(player);

        if (playerInputs.Shoot)
        {
            //Change player state
            entityManager.SetComponentData(player, new StateData
            {
                Value = StateActions.ATTACKING
            });
        }

        //TODO DONT RUN QUERY IF NO ENNEMIES EXISTS
        var playerPos = entityManager.GetComponentData<Translation>(player);
        //Act on all entities with AttackStateData and EnemyTag
        Entities.WithAll<EnemyTag>().ForEach((ref StateData state, in Translation currentPosition, in AttackStateData range) =>
        {
            //Compare distance between current position and target position. If distance <= range -> set state to attack
            if (math.distancesq(currentPosition.Value, playerPos.Value) <= range.Value * range.Value)
            {
                state.Value = StateActions.ATTACKING;
            }
        }).ScheduleParallel();
    }
}