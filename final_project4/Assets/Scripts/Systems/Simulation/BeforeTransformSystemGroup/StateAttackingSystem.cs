using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class StateAttackingSystem : SystemBase
{
    protected override void OnCreate()
    {
        //Debug.Log("Created StateAttackingSystem System...");
        
        //Add system to the group it belongs to
        SimulationSystemGroup simulation = World.GetOrCreateSystem<SimulationSystemGroup>();
        simulation.AddSystemToUpdateList(this);
        simulation.SortSystemUpdateList();
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Updated StateAttackingSystem System...");
        
        //Act on all entities with AttackStateData and EnemyTag
        Entities.WithAll<EnemyTag>().ForEach((ref Translation currentPosition, ref TargetData targetPosition,
            ref AttackStateData range,
            ref StateData state) =>
        {
            //Compare distance between current position and target position. If distance <= range -> set state to attack
            if(math.distancesq(currentPosition.Value, targetPosition.Value) <= range.Value * range.Value)
            {
                state.Value = StateActions.ATTACKING;
            }
        }).ScheduleParallel();
    }
}