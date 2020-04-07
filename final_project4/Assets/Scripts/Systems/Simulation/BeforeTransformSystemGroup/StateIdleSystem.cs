using Unity.Entities;

[DisableAutoCreation]
public class StateIdleSystem : SystemBase
{
    protected override void OnCreate()
    {
        //Debug.Log("Created Idle System...");
        
        //Add system to the group it belongs to
        SimulationSystemGroup simulation = World.GetOrCreateSystem<SimulationSystemGroup>();
        simulation.AddSystemToUpdateList(this);
        simulation.SortSystemUpdateList();
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Updated Idle System...");
        
        //Act on all StateData
        Entities.ForEach((ref StateData state) =>
        {
            //Reset all states to Idle until proven opposite
            state.Value = StateActions.IDLE;
        }).ScheduleParallel();
    }
}
