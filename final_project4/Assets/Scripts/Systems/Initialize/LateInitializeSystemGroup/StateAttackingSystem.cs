using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[DisableAutoCreation]
[AlwaysUpdateSystem]
[UpdateAfter(typeof(StateMovingSystem))]
public class StateAttackingSystem : SystemBase
{

    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        //Player state
        Entity player = GameVariables.Player.Entity;
        InputComponent playerInputs = GetComponent<InputComponent>(player);

        if (playerInputs.Shoot)
        {
            //Change player state
            SetComponent(player, new StateData
            {
                Value = StateActions.ATTACKING
            });
        }

        //TODO DONT RUN QUERY IF NO ENNEMIES EXISTS
        var playerPos = GetComponent<Translation>(player);
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