using Unity.Entities;

[UpdateAfter(typeof(StateAttackingSystem))]
[UpdateAfter(typeof(UpdatePlayerStateSystem))]
public class StateDyingSystem : SystemBase
{
    protected override void OnCreate()
    {
        //Debug.Log("Created Health System...");
        
        //Add system to the group it belongs to
        SimulationSystemGroup simulation = World.GetOrCreateSystem<SimulationSystemGroup>();
        simulation.AddSystemToUpdateList(this);
        simulation.SortSystemUpdateList();
    }

    protected override void OnUpdate()
    {
        //Debug.Log("On HealthUpdate...");

        //Act on all entities with HealthData.
        Entities.ForEach((ref HealthData health, ref StateData state) =>
        {
            //If health <= 0 -> set state to dying
            if (health.Value <= 0)
                state.Value = StateActions.DYING;
        }).ScheduleParallel();
    }
}