using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static GameVariables;

[DisableAutoCreation]
[UpdateAfter(typeof(StateMovingSystem))]
public class StateAttackingSystem : SystemBase
{
    protected override void OnCreate()
    {
        //Debug.Log("Created StateAttackingSystem System...");
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Updated StateAttackingSystem System...");

        //Act on all entities with AttackStateData and EnemyTag
        Entities.WithAll<EnemyTag>().ForEach((ref StateData state, in Translation currentPosition, in AttackStateData range) =>
        {
            //Compare distance between current position and target position. If distance <= range -> set state to attack
            if (math.distancesq(currentPosition.Value, PlayerVars.CurrentPosition) <= range.Value * range.Value)
            {
                state.Value = StateActions.ATTACKING;
            }
        }).ScheduleParallel();
    }
}