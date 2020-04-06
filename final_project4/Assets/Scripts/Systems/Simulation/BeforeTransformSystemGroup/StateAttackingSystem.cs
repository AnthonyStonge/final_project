using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class StateAttackingSystem : SystemBase
{
    private EntityManager entityManager;
    
    protected override void OnCreate()
    {
        this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        //Add system to the group it belongs to
        SimulationSystemGroup simulation = World.GetOrCreateSystem<SimulationSystemGroup>();
        simulation.AddSystemToUpdateList(this);
        simulation.SortSystemUpdateList();
    }

    protected override void OnUpdate()
    {
        //Act on all entities with AttackStateData
        Entities.ForEach((ref Translation currentPosition, ref TargetData targetPosition, ref AttackStateData range,
            ref StateData state) =>
        {
            //Compare distance between current position and target position. If distance <= range -> set state to attack
            if (math.distance(currentPosition.Value, targetPosition.Value) <= range.Value)
            {
                state.Value = FellowActions.ATTACKING;
            }
        }).ScheduleParallel();
    }
}